using SavingTracker.UI.Models.CRUDs;

namespace SavingTraker.App.Interfaces
{
    public interface IMemberService
    {
       Task<List<MemberModel>> GetAll(CancellationToken cancellationToken);
       Task<MemberModel?> GetById(int Id, CancellationToken cancellationToken);
       Task<int> UpSert(MemberModel dto, CancellationToken cancellationToken);
       Task<int> Delete(int Id, CancellationToken cancellationToken);
       Task<List<int>> BulkUpsert(List<MemberModel> dtos, CancellationToken cancellationToken);
    }
}
