namespace SavingTraker.App.Dtos.CRUDs
{
    public class ContributionTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int Frequency { get; set; }
    }
}
