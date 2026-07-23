using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.AppointmentDto
{
    public class AvailableDoctorSlot
    {
        public int doctorId {  get; set; }
        public string doctorName { get; set; }
        public List<DateTime>? dateAvailable { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        
    }
}
