namespace SavingTracker.Api.Models
{
    public class AuthResponseBaseDto
    {
        /// <summary>
        /// Whether the logout operation was successful.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Message describing the result of the logout operation.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
