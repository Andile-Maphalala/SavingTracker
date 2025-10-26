
namespace SavingTracker.Data.Models
{
    public class ContributionType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int Frequency { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
