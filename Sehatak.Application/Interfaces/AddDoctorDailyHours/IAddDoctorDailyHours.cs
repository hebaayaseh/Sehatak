using Sehatak.Application.DTOs.AddDoctorDailyHour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.AddDoctorDailyHours
{
    public interface IAddDoctorDailyHours
    {
        Task<AddDoctorDailyHoursResponse> AddDoctorDailyHoursAsync(int centerId , int userId , int doctorId ,AddDoctorDailyHoursRequest request);
    }
}
