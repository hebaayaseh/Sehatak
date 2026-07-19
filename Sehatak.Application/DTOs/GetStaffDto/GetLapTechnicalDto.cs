using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.GetStaffDto
{
    public class GetLapTechnicalDto
    {
        public int LapTechnicalId { get; set; }
        public string LapTechnicalName { get; set; }
        public List<SummaryShiftDto> LabTechnicalShift { get; set; }
    }
}
