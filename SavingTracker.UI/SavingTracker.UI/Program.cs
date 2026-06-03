
using SavingTracker.UI;
using SavingTracker.UI.Components;


var builder = WebApplication.CreateBuilder(args);

// ============ SERVICE REGISTRATION ============

// External libraries and frameworks
builder.Services
    .AddExternalServices()
    .AddMapping();

// HTTP clients for API communication
builder.Services.AddHttpClients(builder.Configuration);

// Authentication and authorization
builder.Services.AddAuthenticationServices();

// Blazor components
builder.Services.AddBlazorComponents();

// Application business logic services
builder.Services.AddApplicationServices();

// Utility services
builder.Services.AddUtilityServices();

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

