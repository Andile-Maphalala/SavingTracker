
namespace SavingTracker.Data.Models
{
    public class Contribution
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public virtual Member Member { get; set; }

    }
}
