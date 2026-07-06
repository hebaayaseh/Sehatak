using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.DTOs.RecordPaymentRequestDto;
using Sehatak.Application.Interfaces.ISubscriptionPaymentService;
using System.Security.Claims;

namespace Sehatak.API.Controllers.SuperAdminAndAdmin.SubscriptionPayment
{
    [ApiController]
    [Route("api-record-payment-and-confirm")]
    public class SubscriptionPaymentController : ControllerBase
    {
        private readonly ISubscriptionPayment subscriptionPayment;
        public SubscriptionPaymentController(ISubscriptionPayment subscriptionPayment)
        {
            this.subscriptionPayment = subscriptionPayment;
        }
        [Authorize(Policy = "Admin")]
        [HttpPost("record-payment/{centerId}")]
        public async Task<IActionResult> RecordPayment(int centerId, [FromBody] recordPaymentRequestDto request)
        {
            var result = await subscriptionPayment.RecordPaymentAsync(request, centerId);
            return Ok(result);
        }

        
    }
}
