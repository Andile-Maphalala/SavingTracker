using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SavingTracker.Data;
using SavingTracker.Data.Context;
using SavingTracker.Data.Models;
using SavingTraker.App;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddKeyPerFile("/secrets", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.ConfigureApplicationServices();
var test = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.ConfigureDataServices((DbContextOptionsBuilder options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString"));
}, builder.Configuration);

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = 403;
        return Task.CompletedTask;
    };
});

// Add authentication services
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
});
builder.Services.AddAuthorizationBuilder();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(policy => policy
            .WithOrigins("http://localhost:5000") 
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.ApplyDbMigrations();

app.Run();

