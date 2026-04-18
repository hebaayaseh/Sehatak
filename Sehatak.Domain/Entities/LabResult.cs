using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.PaymentEnums;
using System.ComponentModel.DataAnnotations;

namespace Sehatak.Domain.Entities
{
    public class LabResult
    {
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int TechnicianId { get; set; }

        public string? ResultFileUrl { get; set; }

        public string? AiSummary { get; set; }
        public LabStatus Status { get; set; } = LabStatus.Pending;
        public int? PaymentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        

        // Navigation Properties :
        public Patient Patient { get; set; } = null!;
        public User Technician { get; set; } = null!;
        public Payment? Payment { get; set; }

    }
}
