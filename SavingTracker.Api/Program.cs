using Microsoft.EntityFrameworkCore;
using SavingTracker.Data;
using SavingTraker.App;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddKeyPerFile("/secrets", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureDataServices((DbContextOptionsBuilder options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString"));
}, builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.ApplyDbMigrations();

app.Run();

