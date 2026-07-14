using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.Interfaces.GetSttafInterFace;

namespace Sehatak.API.Controllers.ForAllActors
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
        public async Task<IActionResult> GetDoctorsWithDepartments (int centerId)
        {
            var result = await getStaff.GetDoctorsAsync (centerId);
            return Ok(result);
        }
    }
}
