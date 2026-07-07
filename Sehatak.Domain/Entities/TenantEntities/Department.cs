using System.ComponentModel.DataAnnotations;

namespace Sehatak.Domain.Entities.TenantEntities
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        // Navigation Properties :

        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
