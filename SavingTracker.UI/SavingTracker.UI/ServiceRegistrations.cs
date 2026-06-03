using Mapster;
using MapsterMapper;
using MudBlazor.Services;
using SavingTracker.ApiClient;
using SavingTracker.UI.Services;
using SavingTracker.UI.Services.Interfaces;
using SavingTraker.App.Interfaces;
using System.Net;

namespace SavingTracker.UI
{
    public static class ServiceRegistrations
    {
        /// <summary>
        /// Registers all application business logic services.
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILocalStorageService, LocalStorageService>();
            services.AddScoped<IContributionService, ContributionService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IContributionTypeService, ContributionTypeService>();
            services.AddScoped<ISavingsPlanService, SavingsPlanService>();
            services.AddScoped<IDashboardService, DashboardService>();

            return services;
        }


        /// <summary>
        /// Registers external libraries and UI frameworks.
        /// </summary>
        public static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddMudServices();
            services.AddHttpContextAccessor();

            return services;
        }

        /// <summary>
        /// Registers HTTP clients for API communication.
        /// </summary>
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var apiBaseUrl = configuration["ApiBaseUrl"];

            // API Client (with cookie container for API calls)
            services.AddScoped<CookieContainer>();
            services.AddScoped(sp =>
            {
                var cookieContainer = sp.GetRequiredService<CookieContainer>();
                var httpClient = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };
                return new SavingTrackerApiClient(httpClient);
            });

            // Blazor Server HTTP Client (with cookie support for server-side calls)
            services.AddScoped(sp =>
            {
                var httpContext = sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var request = httpContext?.Request;
                var baseUrl = $"{request?.Scheme}://{request?.Host}";

                var cookieContainer = new CookieContainer();
                if (httpContext != null)
                {
                    foreach (var cookie in httpContext.Request.Cookies)
                    {
                        cookieContainer.Add(new Uri(baseUrl), new Cookie(cookie.Key, cookie.Value));
                    }
                }

                var handler = new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    UseCookies = true
                };

                return new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };
            });

            return services;
        }

        /// <summary>
        /// Registers Mapster for object mapping.
        /// </summary>
        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.Scan(System.Reflection.Assembly.GetExecutingAssembly());

            var mapperConfig = new Mapper(typeAdapterConfig);
            services.AddSingleton<IMapper>(mapperConfig);

            return services;
        }

        /// <summary>
        /// Registers authentication and authorization services.
        /// </summary>
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddAuthentication("BlazorCookies")
                .AddCookie("BlazorCookies", options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/access-denied";
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.SlidingExpiration = true;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        // Return 401 for API calls instead of redirecting
                        if (context.Request.Path.StartsWithSegments("/api"))
                        {
                            context.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                        context.Response.Redirect(context.RedirectUri);
                        return Task.CompletedTask;
                    };
                });

            services.AddAuthorization();
            services.AddCascadingAuthenticationState();

            return services;
        }

        /// <summary>
        /// Registers Blazor components and rendering modes.
        /// </summary>
        public static IServiceCollection AddBlazorComponents(this IServiceCollection services)
        {
            services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            return services;
        }

        /// <summary>
        /// Registers utility services.
        /// </summary>
        public static IServiceCollection AddUtilityServices(this IServiceCollection services)
        {
            services.AddSingleton<AppCancellationService>();
            services.AddScoped<AuthenticationHelper>();
            services.AddControllers();

            return services;
        }
    }
}
