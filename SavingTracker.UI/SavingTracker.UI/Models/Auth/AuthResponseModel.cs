
namespace SavingTracker.UI.Models.Auth
{
    public class AuthResponseModel : AuthResponseBaseModel
    {
        public UserInfoModel User { get; set; }

        public List<string> Errors { get; set; } = new();
    }
}
