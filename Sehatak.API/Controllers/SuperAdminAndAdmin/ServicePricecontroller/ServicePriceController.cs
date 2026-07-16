using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.DTOs.ServicePriceDto;
using Sehatak.Application.Interfaces.ServicePriceInterface;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Sehatak.API.Controllers.SuperAdminAndAdmin.ServicePricecontroller
{
    [ApiController]
    [Route("api-service-price")]
    public class ServicePriceController : ControllerBase
    {
        private readonly IServicePrice servicePrice;
        public ServicePriceController(IServicePrice servicePrice)
        {
            this.servicePrice = servicePrice;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("add-service-price/{centerId}")]
        public async Task<IActionResult> AddSrevicePrice(int centerId,ServicePriceRequest request)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await servicePrice.AddServicePriceAsync(userId, centerId, requesr);
            return Ok(request);
        }
    }
}
