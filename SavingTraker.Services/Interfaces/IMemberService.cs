using SavingTraker.App.Dtos.CRUDs;

namespace SavingTraker.App.Interfaces
{
    public interface IMemberService
    {
       Task<List<MemberDto>> GetAll(CancellationToken cancellationToken);
       Task<MemberDto?> GetById(int Id, CancellationToken cancellationToken);
       Task<int> UpSert(MemberDto dto, CancellationToken cancellationToken);
       Task<int> Delete(int Id, CancellationToken cancellationToken);
       Task<List<int>> BulkUpsert(List<MemberDto> dtos, CancellationToken cancellationToken);
    }
}
