using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.DTOs.EditProfile;
using Sehatak.Application.DTOs.SuperAdminProfile;
using Sehatak.Application.Interfaces.ISuperDaminProfile;

namespace Sehatak.API.Controllers.SuperAdmin.SuperAdminProfile
{
    [ApiController]
    [Route("super-admin-profile")]
    public class SuperAdminProfileController : ControllerBase
    {
        private readonly IProfile profile;
        public SuperAdminProfileController(IProfile profile)
        {
            this.profile = profile;
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpGet("super-admin-view-profile/{superAdminId}")]

        public async Task<IActionResult> superAdminViewProfile(int superAdminId)
        {
            var reault = await profile.ViewProfile(superAdminId);
            return Ok(reault);
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPost("super-admin-edit-email/{superAdminId}")]
        public async Task<IActionResult> EditEmail(int superAdminId , [FromBody]EditEmailRequest request)
        {
            var result = await profile.EditEmail(superAdminId, request);
            return Ok(result);
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPost("super-admin-edit-password/{superAdminId}")]
        public async Task<IActionResult> EditPassword(int superAdminId, [FromBody] EditPasswordRequest request)
        {
            var result = await profile.EditPassword(superAdminId, request);
            return Ok(result);
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPost("super-admin-edit-name/{superAdminId}")]
        public async Task<IActionResult> EditName(int superAdminId, [FromBody] EditNameRequest request)
        {
            var result = await profile.EditName(superAdminId, request);
            return Ok(result);
        }

        [Authorize(Policy = "SuperAdminOnly")]
        [HttpPost("super-admin-edit-profile-image/{superAdminId}")]
        public async Task<IActionResult> EditProfileImage(int superAdminId, [FromBody] EditProfileImageRequest request)
        {
            var result = await profile.EditProfileImage(superAdminId, request);
            return Ok(result);
        }

    }
}
