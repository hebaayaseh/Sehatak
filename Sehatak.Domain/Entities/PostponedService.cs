using Sehatak.Domain.Enums.PostponeEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Entities
{
    public class PostponedService
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ReceptionistId { get; set; }
        public PostponeType Type { get; set; }

        public int? AppointmentId { get; set; }    
        public int? ConsultationId { get; set; }    
        public string? Reason { get; set; }
        public DateOnly? NewDate { get; set; }
        public PostponeStatus Status { get; set; } = PostponeStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Patient Patient { get; set; } = null!;
        public User Receptionist { get; set; } = null!;
        public Appointment? Appointment { get; set; }
        public Consultation? Consultation { get; set; }
    }
}
