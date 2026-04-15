
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sehatak.Domain.Entities
{
    public class Doctorschedule
    {
        
        public int Id { get; set; }
        
        public int DoctorId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
        // The doctor can change that if the doctor wants to cancel appointments
        public bool IsActive { get; set; } = true;
        public int? SlotDurationMinutes { get; set; }
        //  Navigation Properties :
        public Doctor doctor { get; set; } = null!;
    }
}
