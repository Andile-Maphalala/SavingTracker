using MapsterMapper;
using SavingTracker.ApiClient;
using SavingTracker.UI.Models.CRUDs;
using SavingTraker.App.Interfaces;

namespace SavingTracker.UI.Services
{
    public class ContributionService(SavingTrackerApiClient apiClient, IMapper mapper) : IContributionService
    {
        public async Task<List<int>> BulkUpsert(List<ContributionModel> model, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<List<ContributionDto>>(model);
            var result = await apiClient.ApiContributionBulkUpsertAsync(dto, cancellationToken);
            return result.ToList();
        }

        public async Task<int> Delete(int Id, CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiContributionDeleteAsync(Id, cancellationToken);
            return result;
        }

        public async Task<List<ContributionModel>> GetAll(CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiContributionGetAllAsync(cancellationToken);
            var mappedResult = mapper.Map<List<ContributionModel>>(result);
            return mappedResult;
        }

        public async Task<List<ContributionModel>> GetAllByMemberId(int memberId, CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiContributionGetAllByMemberIdAsync(memberId, cancellationToken);
            var mappedResult = mapper.Map<List<ContributionModel>>(result);
            return mappedResult;
        }

        public async Task<ContributionModel> GetById(int Id, CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiContributionGetByIdAsync(Id, cancellationToken);
            var mappedResult = mapper.Map<ContributionModel>(result);
            return mappedResult;
        }

        public async Task<int> UpSert(ContributionModel model, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<ContributionDto>(model);
            var result = await apiClient.ApiContributionUpSertAsync(dto, cancellationToken);
            return result;
        }
    }
}
