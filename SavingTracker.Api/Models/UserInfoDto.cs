namespace SavingTracker.Api.Models
{
    /// <summary>
    /// User information returned after successful authentication.
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// The user's ID.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The user's username.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The user's email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The user's first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// The user's last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// The user's assigned roles.
        /// </summary>
        public List<string> Roles { get; set; } = new();
    }
}
