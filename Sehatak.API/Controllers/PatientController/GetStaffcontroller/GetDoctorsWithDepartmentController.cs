using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.Interfaces.GetSttafInterFace;

namespace Sehatak.API.Controllers.PatientController.GetStaffcontroller
{
    [ApiController]
    [Route("get-doctors-with-departments")]
    public class GetDoctorsWithDepartmentController : ControllerBase
    {
        private readonly IGetStaff getStaff;
        public GetDoctorsWithDepartmentController(IGetStaff getStaff)
        {
            this.getStaff = getStaff;
        }
        
        [HttpGet("get-doctors/{centerId}")]
        public async Task<IActionResult> GetDoctorsWithDepartments(int centerId)
        {
            var result = await getStaff.GetDoctorsAsync(centerId);
            return Ok(result);
        }

        
        [HttpGet("get-doctor/{centerId}/{doctorId}")]
        public async Task<IActionResult> GetDoctor(int centerId,int doctorId)
        {
            var result = await getStaff.GetDoctorAsync(centerId,doctorId);
            return Ok(result);
        }


    }
}
