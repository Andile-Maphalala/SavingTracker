using SavingTracker.UI.Models.Auth;

namespace SavingTracker.UI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, UserInfoModel? User)> LoginAsync(string username, string password, CancellationToken cancellationToken);
        Task LogoutAsync(CancellationToken cancellationToken);
        Task<(bool Success, string Message)> RegisterAsync(string username, string email, string password, string firstName, string lastName, string role, CancellationToken cancellationToken);
        Task<(bool Success, UserInfoModel? User)> GetCurrentUserAsync(CancellationToken cancellationToken);
    }
}
