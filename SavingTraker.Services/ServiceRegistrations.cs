using Microsoft.Extensions.DependencyInjection;
using SavingTraker.App.Interfaces;
using SavingTraker.App.Services;


namespace SavingTraker.App
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IContributionService, ContributionService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IContributionTypeService, ContributionTypeService>();
            services.AddScoped<ISavingsPlanService, SavingsPlanService>();
            services.AddScoped<IDashboardService, DashboardService>();

            return services;
        }
    }
}
