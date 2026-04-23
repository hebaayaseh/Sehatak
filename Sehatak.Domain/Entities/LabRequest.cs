using Sehatak.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Entities
{
    public class LabRequest
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int? AppointmentId { get; set; }
        public string? Notes { get; set; }
        public LabRequestStatus Status { get; set; } = LabRequestStatus.Pending;
        public DateTime RequstedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties :
        public Doctor Doctor { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        public Appointment? Appointment { get; set; }
        public LabResult? LabResult { get; set; }
        public ICollection<LabRequestItem> Items { get; set; } = new List<LabRequestItem>();


    }
}
