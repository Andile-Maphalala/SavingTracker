using System.Text.RegularExpressions;
using SavingTraker.App.Interfaces;

namespace SavingTraker.App.Services
{
    /// <summary>
    /// Service for validating authentication credentials.
    /// </summary>
    public class AuthValidationService : IAuthValidationService
    {
        // ============ VALIDATION CONSTANTS ============
        private const int MinUsernameLength = 3;
        private const int MaxUsernameLength = 50;
        private const int MinPasswordLength = 6;
        private const int MaxPasswordLength = 128;

        /// <summary>
        /// Validates username format and length.
        /// </summary>
        public (bool IsValid, string? ErrorMessage) ValidateUsername(string? username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return (false, "Username is required.");

            username = username.Trim();

            if (username.Length < MinUsernameLength)
                return (false, $"Username must be at least {MinUsernameLength} characters long.");

            if (username.Length > MaxUsernameLength)
                return (false, $"Username cannot exceed {MaxUsernameLength} characters.");

            // Allow alphanumeric, underscores, and hyphens
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_-]+$"))
                return (false, "Username can only contain letters, numbers, underscores, and hyphens.");

            return (true, null);
        }

        /// <summary>
        /// Validates password length and basic complexity.
        /// </summary>
        public (bool IsValid, string? ErrorMessage) ValidatePassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password is required.");

            if (password.Length < MinPasswordLength)
                return (false, $"Password must be at least {MinPasswordLength} characters long.");

            if (password.Length > MaxPasswordLength)
                return (false, $"Password cannot exceed {MaxPasswordLength} characters.");

            return (true, null);
        }

        /// <summary>
        /// Validates email format.
        /// </summary>
        public (bool IsValid, string? ErrorMessage) ValidateEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return (false, "Email is required.");

            email = email.Trim();

            if (!Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
                return (false, "Email format is invalid.");

            return (true, null);
        }
    }
}
