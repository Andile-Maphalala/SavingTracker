
namespace SavingTracker.UI.Models.Summary
{
    public class MemberDashboardSummaryModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public decimal TotalContributedAmount { get; set; }
        public decimal OustandingAmount { get; set; }
        public int PeriodsPaid { get; set; }
        public decimal PeriodFee { get; set; }
        public List<ContributionSummaryModel> AllContributions { get; set; }
    }
}
