using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.DTOs.EditProfile.EditEmailOrPasswored;
using Sehatak.Application.DTOs.EditProfile.EditProfileActors;
using Sehatak.Application.Interfaces.IProfileInterface.ProfileAdmin;
using System.Security.Claims;

namespace Sehatak.API.Controllers.SuperAdminAndAdmin.EditProfilecontroller
{
    [ApiController]
    [Route("api-edit-center-staff-informations")]
    public class EditStaffProfileController : ControllerBase
    {
        private readonly IprofileStaff iprofile;
        public EditStaffProfileController(IprofileStaff iprofile)
        {
            this.iprofile = iprofile;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("edit-center-information/{centerId}")]
        public async Task<IActionResult> EditStaffInformation(int centerId , [FromForm] EditCenterInformationRequest request)
        {
            var result = await iprofile.EditCenterInformation(centerId, request);
            return Ok(result);
        }

        [Authorize(Policy = "MedicalStaff")]
        [HttpPost("edit-staff-information/{centerId}")]
        public async Task<IActionResult> EditStaffInformation(int centerId, [FromForm] EditSttafInformationRequest request)
        {
            var adminId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await iprofile.EditSttafInformation(centerId,adminId, request);
            return Ok(result);
        }

        [Authorize(Policy = "MedicalStaff")]
        [HttpPost("edit-staff-email/{centerId}")]
        public async Task<IActionResult> EditStaffEmail(int centerId, [FromForm] EditEmailRequest request)
        {
            var adminId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await iprofile.RequestEditEmail(centerId, adminId, request);
            return Ok(result);
        }



    }
}
