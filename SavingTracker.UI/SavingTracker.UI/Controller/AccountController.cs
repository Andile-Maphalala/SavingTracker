using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SavingTracker.UI.Models.Auth;
using System.Security.Claims;
using System.Text.Json;

namespace SavingTracker.UI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet("user")]
        [AllowAnonymous]
        public IActionResult GetUser()
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            return Ok(new
            {
                name = User.Identity?.Name,
                isAuthenticated = User.Identity?.IsAuthenticated,
                roles
            });
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] UserInfoModel? request)
        {
            if (request == null)
            {
                return BadRequest("Invalid user data");
            }

            try
            {
                // Create claims for the authenticated user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, request.Id.ToString()),
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim(ClaimTypes.Email, request.Email),
                    new Claim(ClaimTypes.GivenName, request.FirstName),
                    new Claim(ClaimTypes.Surname, request.LastName)
                };

                // Add role claims
                foreach (var role in request.Roles ?? new List<string>())
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                };


                // Sign in the user on the Blazor server
                await HttpContext.SignInAsync(
                   IdentityConstants.ApplicationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Ok(new { success = true, message = "User signed in successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
