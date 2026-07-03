using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.DTOs.RenewSubscription;
using Sehatak.Application.Interfaces.RenewSubscription;

namespace Sehatak.API.Controllers.SuperAdmin.SupscriptionPlans
{
    [ApiController]
    [Route("api/renew-subscription")]
    public class RenewSubscriptionController : ControllerBase
    {
        private readonly IRenewSubscription renewSubscriptionService;
        public RenewSubscriptionController(IRenewSubscription renewSubscriptionService)
        {
            this.renewSubscriptionService = renewSubscriptionService;
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPost("renew-subscription/{centerId}")]
        public async Task<IActionResult> RenewSubscription(int centerId, [FromBody] RenewSubscriptionRequest request)
        {
            var result = await renewSubscriptionService.RenewSubscriptionAsync(centerId, request);
            return Ok(result);
        }
    }
}
