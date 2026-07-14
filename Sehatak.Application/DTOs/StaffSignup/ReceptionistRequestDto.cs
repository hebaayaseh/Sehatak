using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.StaffSignup
{
    public class ReceptionistRequestDto
    {
        public string ReceptionistFirstName { get; set; }
        public string ReceptionistLastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? phoneNumber { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public IFormFile? ProfileImage { get; set; }
    }
}
