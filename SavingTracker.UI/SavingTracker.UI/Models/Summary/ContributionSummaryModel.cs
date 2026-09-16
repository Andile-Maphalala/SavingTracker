

using SavingTracker.UI.Models.Common;
using SavingTracker.UI.Services.Helpers;

namespace SavingTracker.UI.Models.Summary
{
    public class ContributionSummaryModel
    {
        [CustomColumn(DisplayName = "Id", Order = 0, Visible = false)]
        public int Id { get; set; }

        [CustomColumn(DisplayName = "Date", Order = 1, FormatString = "d MMM yyyy")]
        public DateTime Date { get; set; }

        [CustomColumn(DisplayName = "Amount", Order = 2, FormatString = "N2", ConditionalColor = ColorHelper.Green)]
        public decimal Amount { get; set; }
    }
}
