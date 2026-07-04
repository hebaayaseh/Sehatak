using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.Interfaces.CentersStatus;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Services.CenterService;

namespace Sehatak.API.Controllers.SuperAdmin.CenterStatus
{
    [ApiController]
    [Route("api/center-status")]
    public class CenterStatusController : ControllerBase
    {
        private readonly ICentersStatus centerStatusService;
        public CenterStatusController(ICentersStatus centerStatusService)
        {
            this.centerStatusService = centerStatusService;
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPatch("suspened-center/{centerId}")]
        public async Task<IActionResult> SuspendedCenter(int centerId)
        {
            var result = await centerStatusService.SuspendedCenter(centerId);
            return Ok(result);
        }


        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPatch("active-center/{centerId}")]
        public async Task<IActionResult> ActiveCenter(int centerId)
        {
            var result = await centerStatusService.ActiveCenter(centerId);
            return Ok(result);
        }

    }
}
