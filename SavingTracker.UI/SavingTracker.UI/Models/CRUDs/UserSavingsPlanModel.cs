namespace SavingTracker.UI.Models.CRUDs
{
    public class UserSavingsPlanModel
    {
        public string UserId { get; set; }
        public IEnumerable<int> SavingsPlanIds { get; set; } = new List<int>();
    }
}
