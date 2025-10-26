using Microsoft.AspNetCore.Mvc;
using SavingTraker.App.Dtos.CRUDs;
using SavingTraker.App.Interfaces;

namespace SavingTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingsPlanController(ISavingsPlanService service) : ControllerBase
    {

        [HttpGet(nameof(GetAll))]
        public async Task<ActionResult<List<SavingsPlanDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await service.GetAll(cancellationToken);
            return Ok(result);
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<SavingsPlanDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await service.GetById(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost(nameof(UpSert))]
        public async Task<ActionResult<int>> UpSert([FromBody] SavingsPlanDto dto, CancellationToken cancellationToken)
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
    }
}
