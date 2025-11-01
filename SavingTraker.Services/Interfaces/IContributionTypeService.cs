using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Dtos.Lookup;

namespace SavingTraker.App.Interfaces
{
    public interface IContributionTypeService
    {
       Task<List<ContributionTypeDto>> GetAll(CancellationToken cancellationToken);
       Task<ContributionTypeDto?> GetById(int Id, CancellationToken cancellationToken);
       Task<int> UpSert(ContributionTypeDto dto, CancellationToken cancellationToken);
       Task<int> Delete(int Id, CancellationToken cancellationToken);
       List<LookUpDto> GetContributionFrequenyList();
    }
}
