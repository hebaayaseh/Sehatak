using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.DTOs.CreateCenterRequestDto;
using Sehatak.Application.Interfaces.CenterRegistrationRequest;

namespace Sehatak.API.Controllers.SuperAdminAndAdmin.CenterRegisterationRequest
{
    [ApiController]
    [Route("api/center-registration")]
    public class CenterRegistrationController :ControllerBase
    {
        private readonly ICenterRegistration centerRegistration;

        public CenterRegistrationController(ICenterRegistration centerRegistration)
        {
            this.centerRegistration = centerRegistration;
        }

        [HttpPost("request")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] CenterRegistrationRequestDto request)
        {
            var result = await centerRegistration.CenterRequestAsync(request);
            return Ok(result);
        }

        [HttpGet("status/{requestId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStatus(int requestId)
        {
            var result = await centerRegistration.GetCenterRegistrationAsync(requestId);
            return Ok(result);
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpGet("get-pending-register-centers")]
        public async Task<IActionResult> GetPending()
        {
            var result = await centerRegistration.GetCentersRegisterationAsync();
            return Ok(result);
        }

        [HttpGet("get-pending-register-center/{requestId}")]
        public async Task<IActionResult> GetById(int requestId)
        {
            var result = await centerRegistration.GetCenterRegistrationAsync(requestId);
            return Ok(result);
        }

        [HttpPost("{requestId}/approve")]
        public async Task<IActionResult> Approve(int requestId)
        {
            int CurrentSuperAdminId = int.Parse(User.FindFirst("SuperAdminId")!.Value);
            await centerRegistration.ApproveCenterRequest(requestId, CurrentSuperAdminId);
            return Ok(new { message = "Center approved and provisioned successfully." });
        }

        [HttpPost("{requestId}/reject")]
        public async Task<IActionResult> Reject(int requestId, [FromBody] RejectCenterRequestDto dto)
        {
            int CurrentSuperAdminId = int.Parse(User.FindFirst("SuperAdminId")!.Value);
            await centerRegistration.RejectAsync(requestId, CurrentSuperAdminId, dto.rejectionReason);
            return Ok(new { message = "Center registration rejected." });
        }

    }
}
