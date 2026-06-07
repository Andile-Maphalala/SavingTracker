using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SavingTracker.Api.Models;
using SavingTracker.Data.Models;
using SavingTraker.App.Interfaces;

namespace SavingTracker.Api.Controllers
{
    /// <summary>
    /// Controller for authentication operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IAuthValidationService validationService,
        ILogger<AuthController> logger) : ControllerBase
    {
        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="request">The login request containing username and password.</param>
        /// <returns>Authentication response with user information if successful.</returns>
        [EnableRateLimiting("login")]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromForm] LoginRequestDto request)
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
                // Validate username
                var (usernameValid, usernameError) = validationService.ValidateUsername(request.Username);
                if (!usernameValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = usernameError
                    });
                }

                // Validate password
                var (passwordValid, passwordError) = validationService.ValidatePassword(request.Password);
                if (!passwordValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = passwordError
                    });
                }

                var user = await userManager.FindByNameAsync(request.Username);
                if (user == null || !user.IsActive)
                {
                    logger.LogWarning("Login attempt for invalid or inactive user: {Username}", request.Username);
                    return Unauthorized(new AuthResponseDto 
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
                    var error = new ObjectResult(new AuthResponseDto 
                    { 
                        Success = false, 
                        Message = "Account is locked due to too many failed login attempts." 
                    });

                    error.StatusCode = StatusCodes.Status423Locked;
                }

                return Unauthorized(new AuthResponseDto 
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
        [Authorize]
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
                // Validate username
                var (usernameValid, usernameError) = validationService.ValidateUsername(request.Username);
                if (!usernameValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = usernameError
                    });
                }

                // Validate password
                var (passwordValid, passwordError) = validationService.ValidatePassword(request.Password);
                if (!passwordValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = passwordError
                    });
                }

                // Validate email
                var (emailValid, emailError) = validationService.ValidateEmail(request.Email);
                if (!emailValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = emailError
                    });
                }

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
    }
}
