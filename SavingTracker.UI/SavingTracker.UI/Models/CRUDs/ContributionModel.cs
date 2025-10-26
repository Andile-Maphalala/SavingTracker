namespace SavingTracker.UI.Models.CRUDs
{
    public class ContributionModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
