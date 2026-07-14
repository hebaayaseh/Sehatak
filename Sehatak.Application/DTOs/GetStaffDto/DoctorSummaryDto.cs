using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.GetStaffDto
{
    public class DoctorSummaryDto
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string? Specialization { get; set; }
        public string? ProfileImageUrl { get; set; }
        public bool OnlineEnabled { get; set; }
    }
}
