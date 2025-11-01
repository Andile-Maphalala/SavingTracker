using Microsoft.AspNetCore.Mvc;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Dtos.Lookup;
using SavingTraker.App.Interfaces;
using System.Threading;

namespace SavingTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContributionTypeController(IContributionTypeService service) : ControllerBase
    {
        [HttpGet(nameof(GetAll))]
        public async Task<ActionResult<List<ContributionTypeDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await service.GetAll(cancellationToken);
            return Ok(result);
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<ContributionTypeDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await service.GetById(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost(nameof(UpSert))]
        public async Task<ActionResult<int>> UpSert([FromBody] ContributionTypeDto dto, CancellationToken cancellationToken)
        {
            var result = await service.UpSert(dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete(nameof(Delete))]
        public async Task<ActionResult<int>> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await service.Delete(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet(nameof(GetContributionFrequenyList))]
        public ActionResult<List<LookUpDto>> GetContributionFrequenyList()
        {
            var result = service.GetContributionFrequenyList();
            return Ok(result);
        }
    }
}
