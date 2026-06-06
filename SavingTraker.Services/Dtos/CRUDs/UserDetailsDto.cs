

namespace SavingTraker.App.Dtos.CRUDs
{
    public class UserDetailsDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedAt { get; set; }
        public string UserName { get; set; }
        public List<string> Role { get; set; }
    }
}
