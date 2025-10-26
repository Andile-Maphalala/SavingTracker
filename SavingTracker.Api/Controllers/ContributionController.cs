using Microsoft.AspNetCore.Mvc;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Interfaces;

namespace SavingTracker.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ContributionController(IContributionService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await service.GetAll(cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByMemberId(int memberId, CancellationToken cancellationToken)
        {
            var result = await service.GetAllByMemberId(memberId, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await service.GetById(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpSert([FromBody] ContributionDto dto, CancellationToken cancellationToken)
        {
            var result = await service.UpSert(dto, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> BulkUpsert([FromBody] List<ContributionDto> dtos, CancellationToken cancellationToken)
        {
            await service.BulkUpsert(dtos, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await service.Delete(id, cancellationToken);
            return Ok(result);
        }
    }
}
