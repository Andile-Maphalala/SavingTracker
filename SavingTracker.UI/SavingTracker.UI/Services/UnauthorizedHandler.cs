using Microsoft.AspNetCore.Components;
using System.Net;

namespace SavingTracker.UI.Services
{
    public class UnauthorizedHandler : DelegatingHandler
    { 
    private readonly NavigationManager _nav;

        public UnauthorizedHandler(NavigationManager nav)
        {
            _nav = nav;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                _nav.NavigateTo("/logout", forceLoad: true);

            return response;
        }
    }
}
