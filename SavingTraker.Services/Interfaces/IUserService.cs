
using SavingTraker.App.Dtos.CRUDs;

namespace SavingTraker.App.Interfaces
{
    public interface IUserService
    {
       Task<List<UserDetailsDto>> ListUsers(CancellationToken cancellationToken);
       Task<UserDetailsDto> GetUserDetails(string id, CancellationToken cancellationToken);
       Task UpdateUserDetails(UserDetailsDto userDetails, CancellationToken cancellationToken);
       Task UpdateUserPassword(UserPasswordDto userPassword, CancellationToken cancellationToken);
       Task UpdateUserRole(UserRoleDto userRole, CancellationToken cancellationToken);
       Task DeleteUser(string id, CancellationToken cancellationToken);
    }
}
