using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.DTOs.CreateCenterRequestDto;
using Sehatak.Application.Interfaces.MedicalCenter;

namespace Sehatak.API.Controllers.SuperAdminController.Centers
{
    [ApiController]
    [Route("create-admin")]
    public class CreateAdminController : ControllerBase 
    {
        private readonly ICreateAdminService createAdminService;
        public CreateAdminController(ICreateAdminService createAdminService)
        {
            this.createAdminService = createAdminService;
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPost("create-admin{centarId}")]
        public async Task<IActionResult> CreateAdminToCenter(int centarId ,CreateAdminRequestDto request)
        {
            var result = await createAdminService.CreateAdminAsync(centarId, request);
            return Ok(result);
        }
    }
}
