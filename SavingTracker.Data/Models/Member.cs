
namespace SavingTracker.Data.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int SavingsPlanId { get; set; }
        public int ContributionTypeId { get; set; }
        public virtual SavingsPlan SavingsPlan { get; set; }
        public virtual ContributionType ContributionType { get; set; }
        public virtual ICollection<Contribution> Contributions { get; set; }
    }
}
