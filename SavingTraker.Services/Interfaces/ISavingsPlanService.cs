using SavingTraker.App.Dtos.CRUDs;

namespace SavingTraker.App.Interfaces
{
    public interface ISavingsPlanService
    {
       Task<List<SavingsPlanDto>> GetAll(CancellationToken cancellationToken);
       Task<SavingsPlanDto> GetById(int Id, CancellationToken cancellationToken);
       Task<int> UpSert(SavingsPlanDto dto, CancellationToken cancellationToken);
       Task<int> Delete(int Id, CancellationToken cancellationToken);
    }
}
