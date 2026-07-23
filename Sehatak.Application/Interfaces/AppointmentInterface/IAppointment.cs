using Sehatak.Application.DTOs.AppointmentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.ApointmentInterface
{
    public interface IAppointment
    {
        Task<AvailableDoctorSlot> GetAvailableDoctorSlot(int centerId, int doctorId);
    }
}
