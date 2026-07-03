using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.DTOs.FeatureCenterDto;
using Sehatak.Application.Interfaces.RemoveFeatureFromCenter;

namespace Sehatak.API.Controllers.SuperAdmin.FeatureOperation
{
    [ApiController]
    [Route("api/remove-feature-from-center")]
    public class RemoveFeatureFromCenterController : ControllerBase
    {
        private readonly IRemoveFeatureFromCenter removeFeatureFromCenterService;
        public RemoveFeatureFromCenterController(IRemoveFeatureFromCenter removeFeatureFromCenterService)
        {
            this.removeFeatureFromCenterService = removeFeatureFromCenterService;
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpDelete("remove-feature-from-center/{centerId}")]
        public async Task<IActionResult> RemoveFeatureFromCenter(int centerId, [FromBody] RemoveFeatureFromCenterRequest featureId)
        {
            var result = await removeFeatureFromCenterService.RemoveFeatureFromCenterAsync(centerId, featureId);
            return Ok(result);
        }

    }
}
