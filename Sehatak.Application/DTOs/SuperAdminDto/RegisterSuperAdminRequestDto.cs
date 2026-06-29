using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.SuperAdminDto
{
    public class RegisterSuperAdminRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string name {  get; set; } = string.Empty;
        [Required]
        [EmailAddress]   
        [MaxLength(150)]
        public string email { get; set; } = string.Empty;
        [Required]
        [Phone]          
        [MaxLength(20)]
        public string phoneNumber { get; set; } = string.Empty;
        [Required]
        public string SuperAdminKey { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; } = DateTime.Now;
        [Required]
        [MinLength(8)]
        public string password { get; set; } 
        
    }
}
