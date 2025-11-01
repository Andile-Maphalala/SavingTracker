using MapsterMapper;
using SavingTracker.ApiClient;
using SavingTracker.UI.Models.Summary;
using SavingTraker.App.Interfaces;

namespace SavingTracker.UI.Services
{
    public class DashboardService(SavingTrackerApiClient apiClient, IMapper mapper) : IDashboardService
    {
        public async Task<DashboardSummaryModel> GetDashboardSummary(int savingPlanId, int FrequncyType, CancellationToken cancellationToken)
        {
            var result = await apiClient.ApiDashboardGetDashboardSummaryAsync(savingPlanId, FrequncyType, cancellationToken);
            var mappedResult = mapper.Map<DashboardSummaryModel>(result);
            return mappedResult;
        }
    }
}
