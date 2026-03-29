using Sehatak.Domain.Enums;

namespace Sehatak.Domain.Entities
{
    public class Waitlist
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int ReceptionistId { get; set; } 
        public DateOnly PreferredDate { get; set; }
        public WaitlistStatus Status { get; set; } = WaitlistStatus.Waiting;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public User Receptionist { get; set; }
    }
}
