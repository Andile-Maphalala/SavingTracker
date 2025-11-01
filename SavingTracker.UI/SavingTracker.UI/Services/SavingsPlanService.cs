using MapsterMapper;
using SavingTracker.ApiClient;
using SavingTracker.UI.Models.CRUDs;
using SavingTraker.App.Interfaces;

namespace SavingTracker.UI.Services
{
    public class SavingsPlanService(SavingTrackerApiClient apiClient, IMapper mapper) : ISavingsPlanService
    {
        public async Task<int> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiSavingsPlanDeleteAsync(id, cancellationToken);
            return result;
        }

        public async Task<List<SavingsPlanModel>> GetAll(CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiSavingsPlanGetAllAsync(cancellationToken);
            var mappedResult = mapper.Map<List<SavingsPlanModel>>(result);
            return mappedResult;
        }

        public async Task<SavingsPlanModel> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiSavingsPlanGetByIdAsync(id,cancellationToken);
            var mappedResult = mapper.Map<SavingsPlanModel>(result);
            return mappedResult;
        }

        public async Task<int> UpSert(SavingsPlanModel model, CancellationToken cancellationToken)
        {
            var dto = mapper.Map<SavingsPlanDto>(model);
            var result = await apiClient.ApiSavingsPlanUpSertAsync(dto, cancellationToken);
            return result;
        }
    }
}
