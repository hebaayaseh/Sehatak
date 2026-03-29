using Sehatak.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sehatak.Domain.Entities
{
    public class Consultation
    {
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime ScheduledAt { get; set; }

        public ConsultationStatus Status { get; set; } = ConsultationStatus.Pending;

        public decimal consultationCost { get; set; }

        public string? VideoLink { get; set; }

        public string? Notes { get; set; }

        // Navigation Properties :
        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
    }
}
