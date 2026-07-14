using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.DTOs.PatientCenter;
using Sehatak.Application.Interfaces.IPatientCenter;

namespace Sehatak.API.Controllers.SuperAdminAndAdmin.PtientCentercontroller
{
    [ApiController]
    [Route("api-get-patient")]
    public class GetPatientCenterController : ControllerBase
    {
        private readonly IGetpatientCenter getpatient;
        public GetPatientCenterController(IGetpatientCenter getpatient)
        {
            this.getpatient = getpatient;
        }

        [Authorize(Policy = "AdminOrAbove")]
        [HttpGet("get-patients-from-center/{centerId}")]
        public async Task<IActionResult> GetPatientsAsync(int centerId)
        {
            var result = await getpatient.GetPatientesAsync(centerId);
            return Ok(result);
        }
        [Authorize(Policy = "AdminOrAbove")]
        [HttpGet("get-patient-from-center/{centerId}")]
        public async Task<IActionResult> GetPatientAsync(int centerId, [FromBody] GetPatientRequestDto request)
        {
            var result = await getpatient.GetPatientAsync(centerId , request);
            return Ok(result);
        }

    }
}
