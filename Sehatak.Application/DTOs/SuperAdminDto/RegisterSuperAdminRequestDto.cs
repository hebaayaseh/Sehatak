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
        public string name {  get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string SuperAdminKey { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string password { get; set; }
        public bool isActive { get; set; } 
    }
}
