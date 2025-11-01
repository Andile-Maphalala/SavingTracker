

using SavingTracker.UI.Models.Summary;

namespace SavingTraker.App.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryModel> GetDashboardSummary(int savingPlanId, int FrequncyType, CancellationToken cancellationToken);
    }
}
