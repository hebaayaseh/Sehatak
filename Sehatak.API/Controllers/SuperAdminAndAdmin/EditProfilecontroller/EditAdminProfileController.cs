using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.DTOs.EditProfile.EditProfileActors;
using Sehatak.Application.Interfaces.IProfileInterface.ProfileAdmin;

namespace Sehatak.API.Controllers.SuperAdminAndAdmin.EditProfilecontroller
{
    [ApiController]
    [Route("api-edit-center-admin-informations")]
    public class EditAdminProfileController : ControllerBase
    {
        private readonly IprofileAdmin iprofile;
        public EditAdminProfileController(IprofileAdmin iprofile)
        {
            this.iprofile = iprofile;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("edit-center-information/{centerId}")]
        public async Task<IActionResult> EditCenterInformation(int centerId , [FromForm] EditCenterInformationRequest request)
        {
            var result = await iprofile.EditCenterInformation(centerId, request);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("edit-admin-information/{centerId}")]
        public async Task<IActionResult> EditAdminiInformation(int centerId, [FromForm] EditSttafInformationRequest request)
        {
            var adminId = int.Parse(User.FindFirst("UserId")!.Value);
            var result = await iprofile.EditSttafInformation(centerId,adminId, request);
            return Ok(result);
        }
    }
}
