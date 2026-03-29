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
        public string Name { get; set; }
        public string Email { get; set; }
        public string phone {  get; set; }
        public userRole role { get; set; }
        public string ProfileImageUrl { get; set; }
        public string PasswordHash { get; set; }
        // Navication Proparite :
        public ICollection<MedicalCenter> AddedCenters { get; set; } = new List<MedicalCenter>();
    }
}
