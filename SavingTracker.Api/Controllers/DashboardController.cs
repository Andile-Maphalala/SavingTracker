using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavingTraker.App.Dtos.Summary;
using SavingTraker.App.Interfaces;

namespace SavingTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController(IDashboardService service) : ControllerBase
    {
        [HttpGet(nameof(GetDashboardSummary))]
        public async Task<ActionResult<DashboardSummaryDto>> GetDashboardSummary(int savingPlanId, int FrequncyType, CancellationToken cancellationToken)
        {
            var result = await service.GetDashboardSummary(savingPlanId, FrequncyType, cancellationToken);
            return Ok(result);
        }
    }
}
