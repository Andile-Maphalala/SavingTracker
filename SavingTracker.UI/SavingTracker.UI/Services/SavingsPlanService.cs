using SavingTracker.ApiClient;
using SavingTracker.UI.Models.CRUDs;
using SavingTraker.App.Interfaces;

namespace SavingTracker.UI.Services
{
    public class SavingsPlanService(SavingTrackerApiClient apiClient) : ISavingsPlanService
    {
        public async Task<int> Delete(int Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SavingsPlanModel>> GetAll(CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiSavingsPlanGetAllAsync(cancellationToken: cancellationToken);
            return result;
        }

        public async Task<SavingsPlanModel> GetById(int Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpSert(SavingsPlanModel dto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
