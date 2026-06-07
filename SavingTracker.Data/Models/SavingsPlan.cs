

namespace SavingTracker.Data.Models
{
    public class SavingsPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<UserSavingsPlan> UserSavingsPlans { get; set; }
    }
}
