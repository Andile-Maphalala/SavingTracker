

using SavingTracker.UI.Models.Common;

namespace SavingTracker.UI.Models.Summary
{
    public class ContributionSummaryModel
    {
        [CustomColumn(DisplayName = "Id", Order = 0, Visible = false)]
        public int Id { get; set; }

        [CustomColumn(DisplayName = "Date", Order = 1, FormatString = "dd/MM/yyyy")]
        public DateTime Date { get; set; }

        [CustomColumn(DisplayName = "Amount", Order = 2, FormatString = "N2")]
        public decimal Amount { get; set; }
    }
}
