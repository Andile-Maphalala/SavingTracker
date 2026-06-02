using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using MudBlazor.Services;
using SavingTracker.ApiClient;
using SavingTracker.UI.Components;
using SavingTracker.UI.Services;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMudServices();
builder.Services.ConfigureServices();
var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
// scans the assembly and gets the IRegister, adding the registration to the TypeAdapterConfig
typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
// register the mapper as Singleton service for my application
var mapperConfig = new Mapper(typeAdapterConfig);
builder.Services.AddSingleton<IMapper>(mapperConfig);


builder.Services.AddScoped<CookieContainer>();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

builder.Services.AddScoped(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var baseUrl = configuration["ApiBaseUrl"];
    var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
    return new SavingTrackerApiClient(httpClient);
});

builder.Services.AddScoped(sp =>
{
    var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
    var request = httpContext?.Request;
    var baseUrl = $"{request?.Scheme}://{request?.Host}";

    var cookieContainer = new CookieContainer();

    if (httpContext != null)
    {
        foreach (var cookie in httpContext.Request.Cookies)
        {
            cookieContainer.Add(
                new Uri(baseUrl),
                new Cookie(cookie.Key, cookie.Value));
        }
    }

    var handler = new HttpClientHandler
    {
        CookieContainer = cookieContainer,
        UseCookies = true
    };

    return new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };
});

builder.Services.AddSingleton<AppCancellationService>();


builder.Services.AddAuthentication("BlazorCookies")
    .AddCookie("BlazorCookies", options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Events.OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            }
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();        
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SavingTracker.UI.Client._Imports).Assembly);

app.Run();

