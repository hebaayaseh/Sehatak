using Sehatak.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Entities
{
    public class StaffAttendance
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int StaffShiftId { get; set; }

        public DateOnly AttendanceDate { get; set; }

        public DateTime? CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        public AttendanceStatus attendanceStatus { get; set; } = AttendanceStatus.Absent;

        // Navigation Properties :
        public User User { get; set; } = null!;
        public StaffShift Shift { get; set; } = null!;
    }
}
