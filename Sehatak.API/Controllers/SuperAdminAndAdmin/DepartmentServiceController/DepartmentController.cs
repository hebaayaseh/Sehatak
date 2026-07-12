using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.DTOs.DepartmentDto;
using Sehatak.Application.Interfaces.DepartmentInterface;

namespace Sehatak.API.Controllers.SuperAdminAndAdmin.DepartmentServiceController
{
    [ApiController]
    [Route("api_department_service")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        [Authorize(Policy = "AdminOrAbove")]
        [HttpPost("add-department/{centerId}")]
        public async Task<IActionResult> AddDepartment(int centerId , [FromBody] DepartmentRequestDto request)
        {
            var result = await departmentService.AddDepartmentAsync(centerId,request);
            return Ok(result);
        }

        
    }
}
