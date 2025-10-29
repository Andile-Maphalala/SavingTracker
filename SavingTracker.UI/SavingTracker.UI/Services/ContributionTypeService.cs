using MapsterMapper;
using SavingTracker.ApiClient;
using SavingTracker.UI.Models.CRUDs;
using SavingTracker.UI.Models.Lookups;
using SavingTraker.App.Interfaces;

namespace SavingTracker.UI.Services
{
    public class ContributionTypeService(SavingTrackerApiClient apiClient, IMapper mapper) : IContributionTypeService
    {
        public async Task<int> Delete(int Id, CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiContributionTypeDeleteAsync(Id, cancellationToken);
            return result;
        }

        public async Task<List<ContributionTypeModel>> GetAll(CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiContributionTypeGetAllAsync(cancellationToken);
            var mappedResult = mapper.Map<List<ContributionTypeModel>>(result);
            return mappedResult;
        }

        public async Task<ContributionTypeModel?> GetById(int Id, CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiContributionTypeGetByIdAsync(Id, cancellationToken);
            var mappedResult = mapper.Map<ContributionTypeModel>(result);
            return mappedResult;
        }

        public List<LookUpModel> GetContributionFrequenyList()
        {
            var result = apiClient.ApiContributionTypeGetContributionFrequenyListAsync();
            var mappedResult = mapper.Map<List<LookUpModel>>(result.Result);
            return mappedResult;
        }

        public async Task<int> UpSert(ContributionTypeModel model, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<ContributionTypeDto>(model);
            var result = await apiClient.ApiContributionTypeUpSertAsync(dto, cancellationToken);
            return result;
        }
    }
}
