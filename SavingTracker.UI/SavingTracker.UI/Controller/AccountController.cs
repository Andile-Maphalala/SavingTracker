using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SavingTracker.UI.Models.Auth;
using SavingTracker.UI.Services;
using SavingTracker.UI.Services.Interfaces;
using System.Net;
using System.Security.Claims;

namespace SavingTracker.UI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAuthService authService, AppCancellationService appCancellationService, IConfiguration configuration) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password, [FromForm] string returnUrl = "/")
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return Redirect($"/login?error=invalid&returnUrl={Uri.EscapeDataString(returnUrl ?? "/")}");
            var apiBaseUrl = configuration["ApiBaseUrl"];
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
            });

            var httpClient = new HttpClient(new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer(),
                AllowAutoRedirect = false
            });

            var response = await httpClient.PostAsync($"{apiBaseUrl}/api/auth/login", formContent);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return Redirect($"/login?error=invalid_credentials&returnUrl={Uri.EscapeDataString(returnUrl ?? "/")}");

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                return Redirect($"/login?error=too_many_attempts&returnUrl={Uri.EscapeDataString(returnUrl ?? "/")}");

            if (response.StatusCode == System.Net.HttpStatusCode.Locked)
                return Redirect($"/login?error=locked&returnUrl={Uri.EscapeDataString(returnUrl ?? "/")}");

            if (!response.IsSuccessStatusCode)
                return Redirect($"/login?error=server_error&returnUrl={Uri.EscapeDataString(returnUrl ?? "/")}");

            var stringResponse = await response.Content.ReadAsStringAsync();
            AuthResponseModel model = new AuthResponseModel();
            model = System.Text.Json.JsonSerializer.Deserialize<AuthResponseModel>(stringResponse, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new AuthResponseModel();

            var user = model.User;
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
            };

            foreach (var role in user.Roles ?? new List<string>())
                claims.Add(new Claim(ClaimTypes.Role, role));

            await HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme)),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true
                });

            return LocalRedirect(returnUrl ?? "/");
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout([FromForm] string returnUrl = "/login")
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return LocalRedirect(returnUrl);
        }
    }
}
