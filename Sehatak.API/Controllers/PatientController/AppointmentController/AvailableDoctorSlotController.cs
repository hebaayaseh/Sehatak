using Microsoft.AspNetCore.Mvc;
using Sehatak.Application.Interfaces.ApointmentInterface;
using Sehatak.Infrastructure.Services.AppointmentService;

namespace Sehatak.API.Controllers.PatientController.AppointmentController
{
    [ApiController]
    [Route("api-get-available-doctor-slot")]
    public class AvailableDoctorSlotController : ControllerBase
    {
        private readonly IAppointment slotService;
        public AvailableDoctorSlotController(IAppointment slotService)
        {
            this.slotService = slotService;
        }

        [HttpPost("available-doctor-slot/{centerId}/{doctorId}")]
        public async Task<IActionResult> AvailableDoctorSlot(int centerId , int doctorId , [FromBody] DateOnly date)
        {
            var result = await slotService.GetAvailableDoctorSlot(centerId , doctorId , date);
            return Ok(result);
        }
    }
}
