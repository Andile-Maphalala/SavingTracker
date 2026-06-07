using Microsoft.AspNetCore.Components;

namespace SavingTracker.UI.Services
{
    public class AuthenticationHelper
    {
        private readonly NavigationManager _navigationManager;

        public AuthenticationHelper(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        /// <summary>
        /// Redirects to login if user is not authenticated.
        /// Call this from MainLayout or pages that need auth checks.
        /// </summary>
        public void ChallengeAuth(bool isAuthenticated)
        {
            if (!isAuthenticated)
            {
                var currentUri = _navigationManager.Uri;

                if (!currentUri.Contains("/login"))
                {
                    var loginUrl = $"/login?returnUrl={Uri.EscapeDataString(currentUri)}";
                    _navigationManager.NavigateTo(loginUrl, forceLoad: true);
                }
            }
        }
    }
}