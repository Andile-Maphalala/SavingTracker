namespace SavingTracker.UI.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        public bool IsLoggedIn { get; private set; } = false;
        public string CurrentUser { get; private set; } = "";

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public bool Login(string username, string password)
        {
            var admin = _config.GetSection("AdminCredentials");
            if (username == admin["Username"] && password == admin["Password"])
            {
                IsLoggedIn = true;
                CurrentUser = username;
                return true;
            }

            IsLoggedIn = false;
            return false;
        }

        public void Logout()
        {
            IsLoggedIn = false;
            CurrentUser = "";
        }
    }

}
