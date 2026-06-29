using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.SuperAdminDto
{
    public class RegisterSuperAdminRequestDto
    {
        public string name {  get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phoneNumber { get; set; } = string.Empty;
        public string SuperAdminKey { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string password { get; set; } = string.Empty;
        
    }
}
