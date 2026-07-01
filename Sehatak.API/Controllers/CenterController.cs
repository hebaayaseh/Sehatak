using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.DTOs.CreateCenterRequestDto;
using Sehatak.Application.Interfaces.MedicalCenter;
using Sehatak.Infrastructure.Services.CenterService;

namespace Sehatak.API.Controllers
{
    [ApiController]
    [Route("api/centers")]
    public class CenterController : ControllerBase
    {
        private readonly ICenterService centerService;
        public CenterController(ICenterService centerService)
        {
            this.centerService = centerService;
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPost("CreateCenter")]
        public async Task<IActionResult> CreateCenter([FromForm]createCenterRequestDto request)
        {
            var result = await centerService.CreateCenterAsync(request);
            return Ok(result);
        }
    }
}
