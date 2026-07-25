
using SavingTracker.UI.Models.Common;

namespace SavingTracker.UI.Models.Summary
{
    public class MemberDashboardSummaryModel
    {
        [CustomColumn(DisplayName = "EndDate", Order = 0,Visible = false)]
        public int MemberId { get; set; }

        [CustomColumn(DisplayName = "Name", Order = 1)]
        public string MemberName { get; set; }

        [CustomColumn(DisplayName = "Total Contributed", Order = 2, FormatString = "N2")]
        public decimal TotalContributedAmount { get; set; }

        [CustomColumn(DisplayName = "Oustanding Amount", Order = 3, FormatString = "N2")]
        public decimal OustandingAmount { get; set; }

        [CustomColumn(DisplayName = "Periods Paid", Order = 4)]
        public int PeriodsPaid { get; set; }

        [CustomColumn(DisplayName = "Period Fee", Order = 6, FormatString = "N2")]
        public decimal PeriodFee { get; set; }

        [CustomColumn(DisplayName = "Next Payment Date", Order = 5, FormatString = "dd MMMM yyyy")]
        public DateTime NextExpectedDate { get; set; }


        [CustomColumn(Visible = false)]
        public List<ContributionSummaryModel> AllContributions { get; set; }
    }
}
