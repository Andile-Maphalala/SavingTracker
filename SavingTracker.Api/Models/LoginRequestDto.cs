namespace SavingTracker.Api.Models
{
    /// <summary>
    /// Request model for user login.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// The username to authenticate.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The password to authenticate.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Whether to persist the login across sessions.
        /// </summary>
        public bool RememberMe { get; set; } = true;
    }
}
