using Sehatak.Domain.Enums;
using System.ComponentModel.DataAnnotations;


namespace Sehatak.Domain.Entities
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public bool IsEmergency { get; set; } = false;
        public int? ReceptionistId { get; set; } 
        public int RescheduleCount { get; set; } = 0;
        // Queue management properties :
        public DateTime? actualStartTime { get; set; }

        public DateTime? actualEndTime { get; set; }

        public int patientId { get; set; }

        public int doctorId { get; set; }

        public DateOnly appointmentDate { get; set; }

        public TimeOnly? timeSlot { get; set; }

        public int? queueNumber { get; set; }

        public decimal? BillAmount { get; set; }

        public AppointmentStatus appointmentStatus { get; set; } = AppointmentStatus.Pending;

        public decimal prepayment { get; set; } = 0;

        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        public DateTime updateAt {  get; set; } = DateTime.UtcNow;
        public string? cancellationReason { get; set; } 

        //  Navigation Properties :
        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public User? Receptionist { get; set; }
        public Payment Payment { get; set; }
        public MedicalRecord? MedicalRecord { get; set; }
        public DoctorRating? Rating { get; set; }
    }
}
