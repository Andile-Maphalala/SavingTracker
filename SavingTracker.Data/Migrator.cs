using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SavingTracker.Data.Context;
using SavingTracker.Data.Models;


namespace SavingTracker.Data
{
    public static class Migrator
    {
        public static async Task ApplyDbMigrations(this IApplicationBuilder app)
        {
            // Initialize database and roles
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Apply migrations
                if (scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.GetPendingMigrations().Count() > 0)
                    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();

                // Create default roles
                var roles = new[] { "Admin", "Member" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Create default admin user if none exists
                var adminUser = await userManager.FindByNameAsync("admin");
                if (adminUser == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@savingtrackerapp.local",
                        FirstName = "System",
                        LastName = "Administrator",
                        EmailConfirmed = true,
                        IsActive = true
                    };

                    var result = await userManager.CreateAsync(user, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }
        }
    }
}
