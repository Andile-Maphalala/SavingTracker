using MapsterMapper;
using SavingTracker.ApiClient;
using SavingTracker.UI.Models.CRUDs;
using SavingTracker.UI.Services.Interfaces;

namespace SavingTracker.UI.Services
{
    public class UserSavingsPlanService(SavingTrackerApiClient apiClient, IMapper mapper) : IUserSavingsPlanService
    {
        public async Task SaveUserSavingPlanAsync(UserSavingsPlanModel userSavingsPlan, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<UserSavingsPlanDto>(userSavingsPlan);
            await apiClient.ApiUserSavingsPlanSaveUserSavingPlanAsync(dto, cancellationToken);

        }
    }
}
