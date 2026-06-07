using SavingTracker.UI.Models.Auth;

namespace SavingTracker.UI.Services.Interfaces
{
    public interface IAuthService
    {
        Task LogoutAsync(CancellationToken cancellationToken);
        Task<(bool Success, string Message)> RegisterAsync(string username, string email, string password, string firstName, string lastName, string role, CancellationToken cancellationToken);
    }
}
