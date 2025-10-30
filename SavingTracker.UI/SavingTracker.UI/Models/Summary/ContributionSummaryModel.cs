

using SavingTracker.UI.Models.Common;

namespace SavingTracker.UI.Models.Summary
{
    public class ContributionSummaryModel
    {
        [CustomColumn(Visible = false)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
