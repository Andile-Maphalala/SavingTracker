using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavingTracker.UI.Services;
using SavingTracker.UI.Services.Interfaces;
using System.Security.Claims;

namespace SavingTracker.UI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAuthService authService, AppCancellationService appCancellationService) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password, [FromForm] string returnUrl = "/")
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return Redirect($"/login?error=invalid&returnUrl={Uri.EscapeDataString(returnUrl ?? "/")}");

            var (success, user) = await authService.LoginAsync(username, password, appCancellationService.Token);

            if (!success || user == null)
                return Redirect($"/login?error=invalid&returnUrl={Uri.EscapeDataString(returnUrl ?? "/")}");

            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
        };

            foreach (var role in user.Roles ?? new List<string>())
                claims.Add(new Claim(ClaimTypes.Role, role));

            await HttpContext.SignInAsync(
                "BlazorCookies",
                new ClaimsPrincipal(new ClaimsIdentity(claims, "BlazorCookies")),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                    AllowRefresh = true,
                    RedirectUri = returnUrl
                });

            return LocalRedirect(returnUrl ?? "/");
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout([FromForm] string returnUrl = "/login")
        {
            await HttpContext.SignOutAsync("BlazorCookies");
            return LocalRedirect(returnUrl);
        }

        [HttpGet("challenge")]
        [AllowAnonymous]
        public async Task ChallengeAuth(string redirectUri)
        {
            var props = new AuthenticationProperties { RedirectUri = redirectUri };
            await HttpContext.ChallengeAsync("BlazorCookies", props);
        }
    }
}
