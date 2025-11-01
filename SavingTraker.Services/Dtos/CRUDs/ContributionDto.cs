namespace SavingTraker.App.Dtos.CRUDs
{
    public class ContributionDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string? MemberName { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
