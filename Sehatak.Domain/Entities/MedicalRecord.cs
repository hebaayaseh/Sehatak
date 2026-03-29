
using System.ComponentModel.DataAnnotations;

namespace Sehatak.Domain.Entities
{
    public class MedicalRecord
    {
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public int? AppointmentId { get; set; }

        public string? Note { get; set; }
        // وصفة علاج
        public string? Prescription { get; set; }
        public string? Diagnosis { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties :
        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public Appointment? Appointment { get; set; }
    }
}
