namespace SavingTracker.UI.Models.Summary
{
    public class DashboardSummaryModel
    {
        public int TotalMembers { get; set; }
        public decimal TotalContribution { get; set; }
        public decimal TotalOutstandingAmount { get; set; }
        public decimal PeriodIncome { get; set; }
        public List<MemberDashboardSummaryModel> MemberSummaries { get; set; }

    }
}
