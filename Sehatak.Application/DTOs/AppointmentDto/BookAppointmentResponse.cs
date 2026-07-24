using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.AppointmentDto
{
    public class BookAppointmentRespesponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<TimeOnly>? AlternativeSlots { get; set; } 
    }
}
