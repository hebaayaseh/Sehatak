using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("super-admin-edit-profile/{superAdminId}")]

        public async Task<IActionResult> superAdminEditEmail(int superAdminId,[FromBody]EditEmailRequest request)
        {
            var reault = await profile.EditProfile(superAdminId,request);
            return Ok(reault);
        }

    }
}
