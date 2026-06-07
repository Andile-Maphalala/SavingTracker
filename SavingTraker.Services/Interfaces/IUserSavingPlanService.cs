

using SavingTraker.App.Dtos.CRUDs;

namespace SavingTraker.App.Interfaces
{
    public interface IUserSavingPlanService
    {
        Task SaveUserSavingPlanAsync(UserSavingsPlanDto userSavingsPlan, CancellationToken cancellationToken);
        Task<List<SavingsPlanDto>> GetAllUserSavingsPlansAsync(string userId, CancellationToken cancellationToken);
    }
}
