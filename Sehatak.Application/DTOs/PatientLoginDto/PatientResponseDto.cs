using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.PatientLoginDto
{
    public class PatientResponseDto
    {
        public string AccessToken {  get; set; }
        public string RefreshToken { get; set; }
    }
}
