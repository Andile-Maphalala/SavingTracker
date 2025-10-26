using SavingTraker.App.Interfaces;

namespace SavingTracker.UI.Services
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IContributionService, ContributionService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IContributionTypeService, ContributionTypeService>();
            services.AddScoped<ISavingsPlanService, SavingsPlanService>();
            return services;
        }
    }
}
