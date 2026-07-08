using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.StaffLogIn
{
    public class StaffLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string role { get; set; }
    }
}
