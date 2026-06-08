

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SavingTracker.Data.Models;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Exceptions;
using SavingTraker.App.Interfaces;

namespace SavingTraker.App.Services
{
    public class UserService(IUserInfo userInfo, UserManager<ApplicationUser> userManager) : IUserService
    {
        public async Task DeleteUser(string id, CancellationToken cancellationToken)
        {
            if(!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }
            var user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                throw new NotFoundException("User", id);
            }
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.Select(e => e.Description).Aggregate((a, b) => a + ", " + b));
            }
        }

        public async Task<UserDetailsDto> GetUserDetails(string id, CancellationToken cancellationToken)
        {
            if(!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }
            var user = await userManager.FindByIdAsync(id);
            var role = await userManager.GetRolesAsync(user);
            var userDetails = new UserDetailsDto
            {
                Id = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastModifiedAt = user.LastModifiedAt,
                UserName = user.UserName,
                Role = role.ToList()
            };
            return userDetails;
        }

        public async Task<List<UserDetailsDto>> ListUsers(CancellationToken cancellationToken)
        {
            var users = await userManager.Users.ToListAsync();
            var userDetailsList = new List<UserDetailsDto>();
            foreach (var user in users)
            {
                var role = await userManager.GetRolesAsync(user);
                var userDetails = new UserDetailsDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    LastModifiedAt = user.LastModifiedAt,
                    UserName = user.UserName,
                    Role = role.ToList()
                };
                userDetailsList.Add(userDetails);
            }
            return userDetailsList;
        }

        public async Task UpdateUserDetails(UserDetailsDto userDetails, CancellationToken cancellationToken)
        {
            if(!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }
            var user = await userManager.FindByIdAsync(userDetails.Id);
            if(user == null)
            {
                throw new NotFoundException("User", userDetails.Id);
            }
            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;
            user.IsActive = userDetails.IsActive;
            user.LastModifiedAt = DateTime.UtcNow;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.Select(e => e.Description).Aggregate((a, b) => a + ", " + b));
            }

        }

        public async Task UpdateUserPassword(UserPasswordDto userPassword, CancellationToken cancellationToken)
        {
            if (!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }
            var user = await userManager.FindByIdAsync(userPassword.UserId);
            if (user == null)
            {
                throw new NotFoundException("User", userPassword.UserId);
            }

            var result = await userManager.ChangePasswordAsync(user, userPassword.OldPassword, userPassword.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).Aggregate((a, b) => a + ", " + b);
                throw new Exception(errors);
            }
        }

        public async Task UpdateUserRole(UserRoleDto userRole, CancellationToken cancellationToken)
        {
            if (!userInfo.IsAdmin())
            {
                throw new Exception("Access denied. Tried to perform unauthorized action.");
            }
            var user = await userManager.FindByIdAsync(userRole.Id);
            var role = await userManager.GetRolesAsync(user);
            foreach (var r in role)
            {
                if(!userRole.Roles.Contains(r))
                {
                    var result = await userManager.RemoveFromRoleAsync(user, r);
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.Select(e => e.Description).Aggregate((a, b) => a + ", " + b));
                    }
                }
            }
            foreach (var r in userRole.Roles)
            {
                if (!role.Contains(r))
                {
                    var result = await userManager.AddToRoleAsync(user, r);
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.ToString());
                    }
                }
            }
        }
    }
}
