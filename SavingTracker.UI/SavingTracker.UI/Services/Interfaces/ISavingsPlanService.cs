using SavingTracker.UI.Models.CRUDs;

namespace SavingTraker.App.Interfaces
{
    public interface ISavingsPlanService
    {
       Task<List<SavingsPlanModel>> GetAll(CancellationToken cancellationToken);
       Task<SavingsPlanModel> GetById(int Id, CancellationToken cancellationToken);
       Task<int> UpSert(SavingsPlanModel model, CancellationToken cancellationToken);
       Task<int> Delete(int Id, CancellationToken cancellationToken);
    }
}
