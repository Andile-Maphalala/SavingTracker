using MapsterMapper;
using SavingTracker.ApiClient;
using SavingTracker.UI.Models.CRUDs;
using SavingTraker.App.Interfaces;

namespace SavingTracker.UI.Services
{
    public class MemberService(SavingTrackerApiClient apiClient, IMapper mapper) : IMemberService
    {
        public async Task<List<int>> BulkUpsert(List<MemberModel> dtos, CancellationToken cancellationToken)
        {
            var dtosDto = mapper.Map<List<MemberDto>>(dtos);
            var result = await apiClient.ApiMemberBulkUpsertAsync(dtosDto, cancellationToken);
            return result.ToList();
        }

        public async Task<int> Delete(int Id, CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiMemberDeleteAsync(Id, cancellationToken);
            return result;
        }

        public async Task<List<MemberModel>> GetAll(CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiMemberGetAllAsync(cancellationToken);
            var mappedResult = mapper.Map<List<MemberModel>>(result);
            return mappedResult;
        }

        public async Task<MemberModel?> GetById(int Id, CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiMemberGetByIdAsync(Id, cancellationToken);
            var mappedResult = mapper.Map<MemberModel?>(result);
            return mappedResult;
        }

        public async Task<int> UpSert(MemberModel model, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<MemberDto>(model);
            var result = await apiClient.ApiMemberUpSertAsync(dto, cancellationToken);
            return result;
        }
    }
}
