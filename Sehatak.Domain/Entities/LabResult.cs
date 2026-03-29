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

        public decimal LabPayment { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public PaymentMethod PaymentMethod { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        // Navigation Properties :
        public Patient Patient { get; set; } = null!;
        public User Technician { get; set; } = null!;
       
    }
}
