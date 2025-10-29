namespace SavingTraker.App.Dtos.Summary
{
    public class DashboardSummaryDto
    {
        public int TotalMembers { get; set; }
        public decimal TotalContribution { get; set; }
        public decimal TotalOutstandingAmount { get; set; }
        public decimal PeriodIncome { get; set; }
        public List<MemberDashboardSummaryDto> MemberSummaries { get; set; } = new List<MemberDashboardSummaryDto>();

    }
}
