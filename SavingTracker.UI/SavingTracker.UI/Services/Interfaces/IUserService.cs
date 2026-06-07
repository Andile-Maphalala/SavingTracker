using SavingTracker.UI.Models.CRUDs;

namespace SavingTracker.UI.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDetailsModel>> ListUsers(CancellationToken cancellationToken);
        Task<UserDetailsModel> GetUserDetails(string id, CancellationToken cancellationToken);
        Task UpdateUserDetails(UserDetailsModel userDetails, CancellationToken cancellationToken);
        Task UpdateUserPassword(UserPasswordModel userPassword, CancellationToken cancellationToken);
        Task UpdateUserRole(UserRoleModel userRole, CancellationToken cancellationToken);
    }
}
