using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.Interfaces.GetSttafInterFace;

namespace Sehatak.API.Controllers.ForAllActors
{
    [ApiController]
    [Route("get-Receptionist-and-Lap-Technical")]
    public class GetReceptionistController : ControllerBase
    {
        private readonly IGetStaff getStaff;
        public GetReceptionistController(IGetStaff getStaff)
        {
            this.getStaff = getStaff;
        }

        [HttpGet("Get-Receptionist/{centerId}")]
        public async Task<IActionResult> GetReceptionistAsync(int centerId)
        {
            var result = await getStaff.GetReceptionistsAsync(centerId);
            return Ok(result);
        }

        [HttpGet("Get-Lap-Tecnical/{centerId}")]
        public async Task<IActionResult> GetLapTechnicalAsync(int centerId)
        {
            var result = await getStaff.GetLapTechnicalsAsync(centerId);
            return Ok(result);
        }
    }
}
