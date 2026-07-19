using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.GetStaffDto
{
    public class GetReceptionistResponseDto
    {
        public int ReceptionistId { get; set; }
        public string ReceptionistName { get;set; }
        public List<SummaryShiftDto> ReceptionistShift { get; set; }

    }
}
