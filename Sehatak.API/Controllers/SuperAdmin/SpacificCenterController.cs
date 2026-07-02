using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.Interfaces.Centers;
using Sehatak.Application.Interfaces.MedicalCenter;

namespace Sehatak.API.Controllers.SuperAdmin
{
    [ApiController]
    [Route("api/spacific-center")]
    public class SpacificCenterController : ControllerBase
    {
        private readonly ISpasificCenter centerService;
        public SpacificCenterController(ISpasificCenter centerService)
        {
            this.centerService = centerService;
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpGet("GetCenterById/{id}")]
        public async Task<IActionResult> GetCenterById(int id)
        {
            var result = await centerService.GetSpasificCenterById(id);
            return Ok(result);
        }
    }
}
