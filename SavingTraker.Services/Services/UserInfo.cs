using Microsoft.AspNetCore.Http;
using SavingTraker.App.Interfaces;
using System.Security.Claims;

namespace SavingTraker.App.Services
{
    public class UserInfo : IUserInfo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfo(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            return GetClaimValue(ClaimTypes.NameIdentifier);
        }

        public bool IsAdmin()
        {
            return _httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;
        }

        private string GetClaimValue(string claimType)
        {
            return _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(claim => claim.Type == claimType)?.Value;
        }
    }
}
