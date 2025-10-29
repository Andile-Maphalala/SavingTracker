namespace SavingTraker.App.Dtos.CRUDs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int SavingsPlanId { get; set; }
        public string SavingsPlanName { get; set; }
        public int ContributionTypeId { get; set; }
        public string ContributionTypeName { get; set; }
        public decimal Amount { get; set; }
        public int Frequency { get; set; }
        public string FrequencyName { get; set; }

    }
}
