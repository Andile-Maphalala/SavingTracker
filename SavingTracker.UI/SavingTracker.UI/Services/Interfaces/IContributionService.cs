using SavingTracker.UI.Models.CRUDs;

namespace SavingTraker.App.Interfaces
{
    public interface IContributionService
    {
       Task<List<ContributionModel>> GetAll(CancellationToken cancellationToken);
       Task<List<ContributionModel>> GetAllByMemberId(int memberId, CancellationToken cancellationToken);
       Task<ContributionModel> GetById(int Id, CancellationToken cancellationToken);
       Task<int> UpSert(ContributionModel model, CancellationToken cancellationToken);
       Task<int> Delete(int Id, CancellationToken cancellationToken);
       Task<List<int>> BulkUpsert(List<ContributionModel> model, CancellationToken cancellationToken);
    }
}
