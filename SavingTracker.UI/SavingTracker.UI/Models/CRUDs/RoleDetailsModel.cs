namespace SavingTracker.UI.Models.CRUDs
{
    public class RoleDetailsModel
    {
        public RoleDetailsModel(string role, bool isNew = true)
        {
            Role = role;
            IsNew = isNew;
        }
        public string Role { get; set; }
        public bool IsNew { get; set; }
    }
}
