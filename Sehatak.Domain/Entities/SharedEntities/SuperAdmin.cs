using Sehatak.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Entities.SharedEntities
{
    public class SuperAdmin
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]   
        [MaxLength(150)]
        public string Email { get; set; }
        [Required]
        [Phone]          
        [MaxLength(20)]
        public string phone {  get; set; }
        public userRole role { get; set; } 
        public string? ProfileImageUrl { get; set; }
        [Required]
        [MinLength(8)]
        public string PasswordHash { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsActive { get; set; }
        // Navication Proparite :
        public ICollection<MedicalCenter> AddedCenters { get; set; } = new List<MedicalCenter>();
    }
}
