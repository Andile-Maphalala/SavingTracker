using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Interfaces;

namespace SavingTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IUserService service) : Controller
    {
        [HttpGet(nameof(ListUsersAsync))]
        public async Task<ActionResult<List<UserDetailsDto>>> ListUsersAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await service.ListUsers(cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetUserDetailsAsync))]
        public async Task<ActionResult<UserDetailsDto>> GetUserDetailsAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await service.GetUserDetails(id,cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(nameof(UpdateUserDetails))]
        public async Task<ActionResult> UpdateUserDetails(UserDetailsDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await service.UpdateUserDetails(dto, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(nameof(UpdateUserPassword))]
        public async Task<ActionResult> UpdateUserPassword(UserPasswordDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await service.UpdateUserPassword(dto, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(nameof(UpdateUserRole))]
        public async Task<ActionResult> UpdateUserRole(UserRoleDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await service.UpdateUserRole(dto, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(nameof(DeleteUserAsync))]
        public async Task<ActionResult> DeleteUserAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                await service.DeleteUser(id, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
