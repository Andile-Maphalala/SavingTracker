

namespace SavingTraker.App.Dtos.CRUDs
{
    public class UserSavingsPlanDto
    {
        public string UserId { get; set; }
        public List<int> SavingsPlanIds { get; set; } = new List<int>();
    }
}
