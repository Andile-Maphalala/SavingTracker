using SavingTracker.UI.Models.Common;

namespace SavingTracker.UI.Models.CRUDs
{
    public class UserDetailsModel
    {
        [CustomColumn(DisplayName = "Id", Order = 0, Visible = false)]
        public string Id { get; set; }

        [CustomColumn(DisplayName = "Name", Order = 2, Visible = true)]
        public string FirstName { get; set; } = string.Empty;

        [CustomColumn(DisplayName = "Surname", Order = 3, Visible = true)]
        public string LastName { get; set; } = string.Empty;
         
        [CustomColumn(DisplayName = "Is Active", Order = 4, Visible = true)]
        public bool IsActive { get; set; } = true;

        [CustomColumn(DisplayName = "Created Date", Order = 5, Visible = true)]
        public DateTime CreatedAt { get; set; }

        [CustomColumn(DisplayName = "Last Mod Date", Order = 6, Visible = true)]
        public DateTime? LastModifiedAt { get; set; }

        [CustomColumn(DisplayName = "Username", Order = 1, Visible = true)]
        public string UserName { get; set; }

        [CustomColumn(DisplayName = "Roles", Order = 99, Visible = false)]
        public List<string> Role { get; set; }
    }
}
