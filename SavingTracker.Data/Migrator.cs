using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SavingTracker.Data.Context;


namespace SavingTracker.Data
{
    public static class Migrator
    {
        public static void ApplyDbMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

            if (serviceScope.ServiceProvider.GetRequiredService<AppDbContext>().Database.GetPendingMigrations().Count() > 0)
                serviceScope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
        }
    }
}
