using SavingTracker.UI.Models.CRUDs;

namespace SavingTracker.UI.Services.Interfaces
{
    public interface IUserSavingsPlanService
    {
        Task SaveUserSavingPlanAsync(UserSavingsPlanModel userSavingsPlan, CancellationToken cancellationToken);
    }
}
