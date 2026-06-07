

namespace SavingTraker.App.Dtos.CRUDs
{
    public class UserPasswordDto
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
