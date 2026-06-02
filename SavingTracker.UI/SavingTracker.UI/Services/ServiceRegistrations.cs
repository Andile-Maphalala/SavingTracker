using Microsoft.AspNetCore.Components.Authorization;
using SavingTracker.UI.Services.Interfaces;
using SavingTraker.App.Interfaces;

namespace SavingTracker.UI.Services
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

            services.AddScoped<ILocalStorageService, LocalStorageService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IContributionService, ContributionService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IContributionTypeService, ContributionTypeService>();
            services.AddScoped<ISavingsPlanService, SavingsPlanService>();
            services.AddScoped<IDashboardService, DashboardService>();

            services.AddScoped<ApiAuthenticationStateProvider>();


            return services;
        }
    }
}
