using Microsoft.AspNetCore.Components.Authorization;
using SavingTracker.UI.Models.Auth;
using System.Net.Http.Json;
using System.Security.Claims;

namespace SavingTracker.UI.Services
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiAuthenticationStateProvider> _logger;
        private UserInfoModel? _currentUser;

        public ApiAuthenticationStateProvider(HttpClient httpClient, ILogger<ApiAuthenticationStateProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Initializes authentication state by checking the Blazor server's HttpContext.User
        /// </summary>
        public async Task InitializeAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<UserAuthResponse>("/api/account/user");

                if (response?.IsAuthenticated == true)
                {
                    _logger.LogInformation("User {Name} already authenticated.", response.Name);
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                }
                else
                {
                    _currentUser = null;
                    _logger.LogInformation("No authenticated user found.");
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                }
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _currentUser = null;
                _logger.LogInformation("User not authenticated (401).");
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing authentication state");
                _currentUser = null;
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (_currentUser != null)
                {
                    return new AuthenticationState(CreateClaimsPrincipal(_currentUser));
                }

                // Try to get current user from Blazor server
                var response = await _httpClient.GetFromJsonAsync<UserAuthResponse>("/api/account/user");

                if (response?.IsAuthenticated == true)
                {
                    _currentUser = new UserInfoModel
                    {
                        Username = response.Name ?? "",
                        Roles = response.Roles ?? new List<string>()
                    };
                    return new AuthenticationState(CreateClaimsPrincipal(_currentUser));
                }

                return new AuthenticationState(new ClaimsPrincipal());
            }
            catch
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }

        public async Task NotifyLoggedIn(UserInfoModel user)
        {
            _currentUser = user;
            _logger.LogInformation("User {Username} logged in.", user.Username);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            await Task.CompletedTask;
        }

        public async Task NotifyLoggedOut()
        {
            _currentUser = null;
            _logger.LogInformation("User logged out.");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            await Task.CompletedTask;
        }

        private ClaimsPrincipal CreateClaimsPrincipal(UserInfoModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "api");
            return new ClaimsPrincipal(identity);
        }
    }

    public class UserAuthResponse
    {
        public string? Name { get; set; }
        public bool IsAuthenticated { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}