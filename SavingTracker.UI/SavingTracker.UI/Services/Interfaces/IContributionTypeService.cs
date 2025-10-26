using SavingTracker.UI.Models.CRUDs;

namespace SavingTraker.App.Interfaces
{
    public interface IContributionTypeService
    {
       Task<List<ContributionTypeModel>> GetAll(CancellationToken cancellationToken);
       Task<ContributionTypeModel?> GetById(int Id, CancellationToken cancellationToken);
       Task<int> UpSert(ContributionTypeModel dto, CancellationToken cancellationToken);
       Task<int> Delete(int Id, CancellationToken cancellationToken);
    }
}
