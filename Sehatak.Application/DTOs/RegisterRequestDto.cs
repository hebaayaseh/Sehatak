using Sehatak.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs
{
    public class RegisterRequestDto
    {
        public int CenterId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public userRole role { get; set; } = userRole.Patient;
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; } = false;


    }
}
