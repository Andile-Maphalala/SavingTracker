

namespace SavingTracker.Data.Models
{
    public class UserSavingsPlan
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int SavingsPlanId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual SavingsPlan SavingsPlan { get; set; }
    }
}
