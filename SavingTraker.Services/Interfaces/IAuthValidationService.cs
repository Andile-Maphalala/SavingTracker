namespace SavingTraker.App.Interfaces
{
    /// <summary>
    /// Service for validating authentication credentials.
    /// </summary>
    public interface IAuthValidationService
    {
        /// <summary>
        /// Validates username format and length.
        /// </summary>
        (bool IsValid, string? ErrorMessage) ValidateUsername(string? username);

        /// <summary>
        /// Validates password length and basic complexity.
        /// </summary>
        (bool IsValid, string? ErrorMessage) ValidatePassword(string? password);

        /// <summary>
        /// Validates email format.
        /// </summary>
        (bool IsValid, string? ErrorMessage) ValidateEmail(string? email);
    }
}
