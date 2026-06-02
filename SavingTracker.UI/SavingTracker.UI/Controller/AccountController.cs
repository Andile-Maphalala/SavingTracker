using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SavingTracker.UI.Models.Auth;
using SavingTracker.UI.Services;
using SavingTracker.UI.Services.Interfaces;
using System.Security.Claims;
using System.Text.Json;

namespace SavingTracker.UI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAuthService authService, AppCancellationService appCancellationService) : ControllerBase
    {
        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = "/")
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return Redirect($"/login?error=invalid&returnUrl={Uri.EscapeDataString(returnUrl ?? "/")}");

            // Use LoginAsync instead of GetCurrentUserAsync since we're not authenticated yet
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
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                    AllowRefresh = true,
                    RedirectUri = returnUrl
                });

            return LocalRedirect(returnUrl ?? "/");
        }

        [HttpGet("challenge")]
        [AllowAnonymous]
        public async Task ChallengeAuth(string redirectUri)
        {
            var props = new AuthenticationProperties { RedirectUri = redirectUri };
            await HttpContext.ChallengeAsync("BlazorCookies", props);
        }

        [HttpGet("user")]
        [AllowAnonymous]
        public IActionResult GetUser()
        {
            if (User.Identity?.IsAuthenticated != true)
                return Unauthorized();

            var response = new UserAuthResponse
            {
                Name = User.Identity?.Name,
                IsAuthenticated = true,
                Roles = User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList()
            };
            return Ok(response);
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] UserInfoModel request)
        {
            Console.WriteLine($"=== SignIn called for {request?.Username} ===");

            if (request == null)
            {
                Console.WriteLine("=== Request is NULL ===");
                return BadRequest();
            }

            var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, request.Id),
        new(ClaimTypes.Name, request.Username),
        new(ClaimTypes.Email, request.Email),
    };

            foreach (var role in request.Roles ?? new List<string>())
                claims.Add(new Claim(ClaimTypes.Role, role));

            await HttpContext.SignInAsync(
                "BlazorCookies",
                new ClaimsPrincipal(new ClaimsIdentity(claims, "BlazorCookies")),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                    AllowRefresh = true
                });

            Console.WriteLine("=== SignIn completed ===");
            return Ok(new { success = true });
        }
    }
}
