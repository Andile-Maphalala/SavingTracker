using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Interfaces;

namespace SavingTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContributionController(IContributionService service) : ControllerBase
    {
        [HttpGet(nameof(GetAll))]
        public async Task<ActionResult<List<ContributionDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await service.GetAll(cancellationToken);
            return Ok(result);
        }

        [HttpGet(nameof(GetAllByMemberId))]
        public async Task<ActionResult<List<ContributionDto>>> GetAllByMemberId(int memberId, CancellationToken cancellationToken)
        {
            var result = await service.GetAllByMemberId(memberId, cancellationToken);
            return Ok(result);
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<ContributionDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await service.GetById(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost(nameof(UpSert))]
        public async Task<ActionResult<int>> UpSert([FromBody] ContributionDto dto, CancellationToken cancellationToken)
        {
            var result = await service.UpSert(dto, cancellationToken);
            return Ok(result);
        }

        [HttpPost(nameof(BulkUpsert))]
        public async Task<ActionResult<List<int>>> BulkUpsert([FromBody] List<ContributionDto> dtos, CancellationToken cancellationToken)
        {
            var result = await service.BulkUpsert(dtos, cancellationToken);
            return Ok(result);
        }

        [HttpDelete(nameof(Delete))]
        public async Task<ActionResult<int>> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await service.Delete(id, cancellationToken);
            return Ok(result);
        }
    }
}
