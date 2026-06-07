namespace SavingTracker.Api.Models
{
    /// <summary>
    /// Response model for authentication operations.
    /// </summary>
    public class AuthResponseDto : AuthResponseBaseDto
    {

        /// <summary>
        /// The user information if successful.
        /// </summary>
        public UserInfoDto? User { get; set; }

        /// <summary>
        /// List of errors if the operation failed.
        /// </summary>
        public List<string> Errors { get; set; } = new();
    }
}
