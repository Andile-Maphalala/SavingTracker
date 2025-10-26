

using SavingTraker.App.Dtos.Summary;

namespace SavingTraker.App.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetDashboardSummary(int savingPlanId, int FrequncyType, CancellationToken cancellationToken);
    }
}
