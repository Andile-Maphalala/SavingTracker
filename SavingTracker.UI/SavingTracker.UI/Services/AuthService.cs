using MapsterMapper;
using SavingTracker.ApiClient;
using SavingTracker.UI.Services.Interfaces;
using SavingTracker.UI.Models.Auth;

namespace SavingTracker.UI.Services
{
    /// <summary>
    /// Service for handling authentication by calling the API authentication endpoints.
    /// </summary>
    public class AuthService(SavingTrackerApiClient apiClient, IMapper mapper, ILogger<AuthService> logger) : IAuthService
    {

        /// <summary>
        /// Authenticates a user by calling the API login endpoint.
        /// </summary>
        /// <param name="username">The username to authenticate.</param>
        /// <param name="password">The password to authenticate.</param>
        /// <returns>True if authentication was successful, false otherwise.</returns>
        public async Task<(bool Success, UserInfoModel? User)> LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            try
            {
                var loginRequest = new LoginRequestDto { Username = username, Password = password, RememberMe = true };

                var response = await apiClient.ApiAuthLoginAsync(loginRequest, cancellationToken);

                if (response.Success)
                {
                    logger.LogInformation("User {Username} logged in successfully via API.", username);
                    var result = new UserInfoModel
                    {
                        Id = response.User.Id,
                        Username = response.User.Username,
                        Email = response.User.Email,
                        FirstName = response.User.FirstName,
                        LastName = response.User.LastName,
                        Roles = response.User.Roles.ToList() ?? new List<string>()
                    };
                    return (true, result);
                }

                logger.LogWarning("Login failed for user: {Username}", username);
                return (false, null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during login for user: {Username}", username);
                return (false, null);
            }
        }

        /// <summary>
        /// Logs out the current user by calling the API logout endpoint.
        /// </summary>
        public async Task LogoutAsync(CancellationToken cancellationToken)
        {
            try
            {
                var response = await apiClient.ApiAuthLogoutAsync(cancellationToken);

                if (response.Success)
                {
                    logger.LogInformation("User logged out successfully via API.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during logout.");
            }
        }

        /// <summary>
        /// Registers a new user by calling the API register endpoint.
        /// </summary>
        /// <param name="username">The username for the new user.</param>
        /// <param name="email">The email address for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="role">The role to assign to the user (default: Member).</param>
        /// <returns>A tuple containing success status and message.</returns>
        public async Task<(bool Success, string Message)> RegisterAsync(
            string username,
            string email,
            string password,
            string firstName,
            string lastName,
            string role, 
            CancellationToken cancellationToken)
        {
            try
            {
                var registerRequest = new RegisterRequestDto
                {
                    Username = username,
                    Email = email,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    Role = role
                };

                var response = await apiClient.ApiAuthRegisterAsync(registerRequest, cancellationToken);

                var message = response.Message ?? "Registration failed.";

                if (response.Success)
                {
                    logger.LogInformation("User {Username} registered successfully via API.", username);
                    return (true, message);
                }

                return (false, message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during registration for user: {Username}", username);
                return (false, "An error occurred during registration.");
            }
        }


        public async Task<(bool Success, UserInfoModel? User)> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            try
            {
                var response = await apiClient.ApiAuthMeAsync(cancellationToken);

                if (response is not null)
                {
                    var result = new UserInfoModel
                    {
                        Id = response.Id,
                        Username = response.Username,
                        Email = response.Email,
                        FirstName = response.FirstName,
                        LastName = response.LastName,
                        Roles = response.Roles.ToList() ?? new List<string>()
                    };
                    return (true, result);
                }

                return (false, null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting current user.");
                return (false, null);
            }
        }
    }
}
