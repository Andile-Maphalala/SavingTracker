using Microsoft.Extensions.DependencyInjection;
using SavingTraker.App.Interfaces;
using SavingTraker.App.Services;


namespace SavingTraker.App
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            // Authentication & Validation
            services.AddScoped<IAuthValidationService, AuthValidationService>();
            services.AddScoped<IUserInfo, UserInfo>();

            // Business Logic Services
            services.AddScoped<IContributionService, ContributionService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IContributionTypeService, ContributionTypeService>();
            services.AddScoped<ISavingsPlanService, SavingsPlanService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserSavingPlanService, UserSavingPlanService>();

            return services;
        }
    }
}
