using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs
{
    public class VerifyOtpRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;    
    }
}
