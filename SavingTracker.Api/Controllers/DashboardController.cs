using Microsoft.AspNetCore.Mvc;
using SavingTraker.App.Interfaces;

namespace SavingTracker.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DashboardController(IDashboardService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetDashboardSummary(int savingPlanId, int FrequncyType, CancellationToken cancellationToken)
        {
            var result = await service.GetDashboardSummary(savingPlanId, FrequncyType, cancellationToken);
            return Ok(result);
        }
    }
}
