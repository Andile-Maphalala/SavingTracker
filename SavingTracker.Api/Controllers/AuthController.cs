using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SavingTracker.Api.Models;
using SavingTracker.Data.Models;

namespace SavingTracker.Api.Controllers
{
    /// <summary>
    /// Controller for authentication operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager,ILogger<AuthController> logger) : ControllerBase
    {
        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="request">The login request containing username and password.</param>
        /// <returns>Authentication response with user information if successful.</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDto 
                { 
                    Success = false, 
                    Message = "Invalid request data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            try
            {
                var user = await userManager.FindByNameAsync(request.Username);
                if (user == null || !user.IsActive)
                {
                    logger.LogWarning("Login attempt for invalid or inactive user: {Username}", request.Username);
                    return BadRequest(new AuthResponseDto 
                    { 
                        Success = false, 
                        Message = "Invalid username or password." 
                    });
                }

                var result = await signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    var userInfo = new UserInfoDto
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Roles = roles.ToList()
                    };

                    logger.LogInformation("User {RequestUsername} logged in successfully.", request.Username);

                    return Ok(new AuthResponseDto
                    {
                        Success = true,
                        Message = "Login successful.",
                        User = userInfo
                    });
                }

                if (result.IsLockedOut)
                {
                    logger.LogWarning("User {RequestUsername} account is locked.", request.Username);
                    return BadRequest(new AuthResponseDto 
                    { 
                        Success = false, 
                        Message = "Account is locked due to too many failed login attempts." 
                    });
                }

                return BadRequest(new AuthResponseDto 
                { 
                    Success = false, 
                    Message = "Invalid username or password." 
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during login for user: {RequestUsername}", request.Username);
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthResponseDto 
                { 
                    Success = false, 
                    Message = "An error occurred during login." 
                });
            }
        }

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="request">The registration request containing user information.</param>
        /// <returns>Authentication response confirming successful registration.</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDto 
                { 
                    Success = false, 
                    Message = "Invalid request data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            try
            {
                var userExists = await userManager.FindByNameAsync(request.Username);
                if (userExists != null)
                {
                    return BadRequest(new AuthResponseDto 
                    { 
                        Success = false, 
                        Message = "Username already exists." 
                    });
                }

                // Validate role exists
                if (!await roleManager.RoleExistsAsync(request.Role))
                {
                    return BadRequest(new AuthResponseDto 
                    { 
                        Success = false, 
                        Message = $"Role '{request.Role}' does not exist." 
                    });
                }

                var user = new ApplicationUser
                {
                    UserName = request.Username,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    logger.LogWarning("Failed to create user {RequestUsername}: {Errors}", request.Username, string.Join(", ", errors));
                    return BadRequest(new AuthResponseDto 
                    { 
                        Success = false, 
                        Message = "Registration failed.",
                        Errors = errors
                    });
                }

                var roleResult = await userManager.AddToRoleAsync(user, request.Role);
                if (!roleResult.Succeeded)
                {
                    var errors = roleResult.Errors.Select(e => e.Description).ToList();
                    logger.LogWarning("Failed to assign role {RequestRole} to user {RequestUsername}", request.Role, request.Username);
                    return BadRequest(new AuthResponseDto 
                    { 
                        Success = false, 
                        Message = "Failed to assign role.",
                        Errors = errors
                    });
                }

                var roles = await userManager.GetRolesAsync(user);
                var userInfo = new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roles.ToList()
                };

                logger.LogInformation("User {RequestUsername} registered successfully with role {RequestRole}.", request.Username, request.Role);

                return Ok(new AuthResponseDto
                {
                    Success = true,
                    Message = "Registration successful.",
                    User = userInfo
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during registration for user: {RequestUsername}", request.Username);
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthResponseDto 
                { 
                    Success = false, 
                    Message = "An error occurred during registration." 
                });
            }
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>Authentication response confirming logout.</returns>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<AuthResponseBaseDto>> Logout()
        {
            try
            {
                await signInManager.SignOutAsync();
                logger.LogInformation("User logged out successfully.");

                return Ok(new AuthResponseBaseDto
                {
                    Success = true,
                    Message = "Logout successful."
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during logout.");
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthResponseBaseDto
                { 
                    Success = false, 
                    Message = "An error occurred during logout." 
                });
            }
        }

        /// <summary>
        /// Gets the current authenticated user's information.
        /// </summary>
        /// <returns>Current user information if authenticated, otherwise null.</returns>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserInfoDto>> GetCurrentUser()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var roles = await userManager.GetRolesAsync(user);
            return Ok(new UserInfoDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList()
            });
        }
    }
}
