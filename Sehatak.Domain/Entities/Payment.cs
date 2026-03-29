using Sehatak.Domain.Enums.PaymentEnums;

namespace Sehatak.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int? AppointmentId { get; set; }
        public int PatientId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentType Type { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;
        public int? ReceptionistId { get; set; }
        // Navigation Properties : 
        public Appointment? Appointment { get; set; }
        public Patient Patient { get; set; } = null!;
        public User? Receptionist { get; set; }
    }
}
