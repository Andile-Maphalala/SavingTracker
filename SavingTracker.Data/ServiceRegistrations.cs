using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SavingTracker.Data.Context;

namespace SavingTracker.Data
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection ConfigureDataServices(this IServiceCollection services, Action<DbContextOptionsBuilder> configureContext, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(configureContext);
            return services;
        }

    }
}
