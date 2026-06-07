using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Interfaces;

namespace SavingTracker.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserSavingsPlanController(IUserSavingPlanService service) : Controller
    {
        [HttpPost(nameof(SaveUserSavingPlanAsync))]
        public async Task<ActionResult> SaveUserSavingPlanAsync(UserSavingsPlanDto dto,CancellationToken cancellationToken)
        {
            try
            {
                await service.SaveUserSavingPlanAsync(dto, cancellationToken);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(nameof(GetAllUserSavingsPlansAsync))]
        public async Task<ActionResult<List<SavingsPlanDto>>> GetAllUserSavingsPlansAsync(string Id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await service.GetAllUserSavingsPlansAsync(Id, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
