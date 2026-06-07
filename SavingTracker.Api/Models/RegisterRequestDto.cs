namespace SavingTracker.Api.Models
{
    /// <summary>
    /// Request model for user registration.
    /// </summary>
    public class RegisterRequestDto
    {
        /// <summary>
        /// The username for the new user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The email address for the new user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The password for the new user.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// The role to assign to the user (default: Member).
        /// </summary>
        public string Role { get; set; } = "Member";
    }
}
