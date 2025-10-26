using SavingTraker.App.Dtos.CRUDs;

namespace SavingTraker.App.Interfaces
{
    public interface IContributionService
    {
       Task<List<ContributionDto>> GetAll(CancellationToken cancellationToken);
       Task<List<ContributionDto>> GetAllByMemberId(int memberId, CancellationToken cancellationToken);
       Task<ContributionDto> GetById(int Id, CancellationToken cancellationToken);
       Task<int> UpSert(ContributionDto dto, CancellationToken cancellationToken);
       Task<int> Delete(int Id, CancellationToken cancellationToken);
       Task<List<int>> BulkUpsert(List<ContributionDto> dto, CancellationToken cancellationToken);
    }
}
