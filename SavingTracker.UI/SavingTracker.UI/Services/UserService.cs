using MapsterMapper;
using SavingTracker.ApiClient;
using SavingTracker.UI.Models.CRUDs;
using SavingTracker.UI.Services.Interfaces;

namespace SavingTracker.UI.Services
{
    public class UserService(SavingTrackerApiClient apiClient, IMapper mapper) : IUserService
    {
        public async Task<UserDetailsModel> GetUserDetails(string id, CancellationToken cancellationToken)
        {
            var dto = await apiClient.ApiUserGetUserDetailsAsync(id, cancellationToken);
            var result = mapper.Map<UserDetailsModel>(dto);
            return result;
        }

        public async Task<List<UserDetailsModel>> ListUsers(CancellationToken cancellationToken)
        {
            var dtos = await apiClient.ApiUserListUsersAsync(cancellationToken);
            var results = dtos.Select(dto => mapper.Map<UserDetailsModel>(dto)).ToList();
            return results;
        }

        public async Task UpdateUserDetails(UserDetailsModel userDetails, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<UserDetailsDto>(userDetails);
            await apiClient.ApiUserUpdateUserDetailsAsync(dto, cancellationToken);
        }

        public async Task UpdateUserPassword(UserPasswordModel userPassword, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<UserPasswordDto>(userPassword);
            await apiClient.ApiUserUpdateUserPasswordAsync(dto, cancellationToken);
        }

        public async Task UpdateUserRole(UserRoleModel userRole, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<UserRoleDto>(userRole);
            await apiClient.ApiUserUpdateUserRoleAsync(dto, cancellationToken);
        }
    }
}
