namespace SavingTracker.UI.Models.CRUDs
{
    public class ContributionTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int Frequency { get; set; }
        public string FrequencyName { get; set; }

    }
}
