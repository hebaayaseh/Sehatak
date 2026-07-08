using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.PatienRegisterDto
{
    public class VerifyOtpRequestDto
    {

        public int UserId { get; set; }
        public string code { get; set; } = string.Empty;
    }
}
