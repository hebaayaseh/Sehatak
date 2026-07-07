namespace Sehatak.Domain.Entities.TenantEntities
{
    public class DoctorRating
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public int PatientId { get; set; }
        // The appointment must be completed
        public int AppointmentId { get; set; }

        public int Rating { get; set; }

        public string? Review { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } 

        //  Navigation Properties :
        public Doctor Doctor { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        public Appointment Appointment { get; set; } = null!;
    }
}
