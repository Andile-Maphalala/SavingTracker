using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SavingTracker.UI.Models.Auth;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;

namespace SavingTracker.UI.Services
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly ILogger<ApiAuthenticationStateProvider> _logger;
        private AppCancellationService _cancellationService;
        public ApiAuthenticationStateProvider(
            HttpClient httpClient,
            NavigationManager navigationManager,
            ILogger<ApiAuthenticationStateProvider> logger,
            AppCancellationService cancellationService)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _logger = logger;
            _cancellationService = cancellationService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/account/user", _cancellationService.Token);
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserAuthResponse>();
                    if (user?.IsAuthenticated == true)
                    {
                        return BuildAuthState(user);
                    }
                }
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Not authenticated — challenge will handle redirect
            }
            catch (System.Text.Json.JsonException ex)
            {
                _logger.LogError(ex, "API returned invalid JSON. Check if server is returning error pages instead of JSON.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting auth state");
            }

            return Anonymous();
        }

        // Called from MainLayout.OnAfterRenderAsync
        public async Task<AuthenticationState> ChallengeAuth()
        {
            var authState = await GetAuthenticationStateAsync();

            if (authState?.User?.Identity?.IsAuthenticated != true)
            {
                var currentUri = _navigationManager.Uri;

                // Prevent redirect loop
                if (!currentUri.Contains("/login") &&
                    !currentUri.Contains("/api/account/challenge"))
                {
                    _navigationManager.NavigateTo(
                        $"/api/account/challenge?redirectUri={Uri.EscapeDataString(currentUri)}",
                        forceLoad: true);
                }
            }

            return authState;
        }

        public async Task NotifyLoggedIn(UserInfoModel user)
        {
            var userAuthResponse = new UserAuthResponse
            {
                Name = user.Username,
                IsAuthenticated = true,
                Roles = user.Roles ?? new List<string>()
            };
            NotifyAuthenticationStateChanged(Task.FromResult(BuildAuthState(userAuthResponse)));
            await Task.CompletedTask;
        }

        public async Task NotifyLoggedOut()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(Anonymous()));
            await Task.CompletedTask;
        }

        private static AuthenticationState BuildAuthState(UserAuthResponse user)
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
        };
            foreach (var role in user.Roles ?? new List<string>())
                claims.Add(new Claim(ClaimTypes.Role, role));

            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity(claims, "BlazorCookies")));
        }

        private static AuthenticationState Anonymous()
            => new(new ClaimsPrincipal(new ClaimsIdentity()));
    }
    public class UserAuthResponse
    {
        public string? Name { get; set; }
        public bool IsAuthenticated { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}