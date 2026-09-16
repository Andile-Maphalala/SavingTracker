using SavingTracker.UI.Models.Common;
using SavingTracker.UI.Services.Helpers;

namespace SavingTracker.UI.Models.Summary
{
    public class MemberOutstandingSummaryModel
    {
        [CustomColumn(DisplayName = "Name", Order = 0, Visible = true)]
        public string Name { get; set; }

        [CustomColumn(DisplayName = "Amount", Order = 2, FormatString = "N2" , ConditionalColor = ColorHelper.Red)]
        public decimal Amount { get; set; }

        [CustomColumn(DisplayName = "Next Payment Date", Order = 1, FormatString = "d MMM yyyy")]
        public DateTime Date { get; set; }
    }
}
