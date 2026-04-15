using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Entities
{
    public class ServicePrice
    {
        
            public int Id { get; set; }
            public int ServiceTypeId { get; set; } 
            public string Name { get; set; } = string.Empty;        // "جلسة أونلاين", "تحليل "
            public decimal Price { get; set; }
            public bool IsActive { get; set; } = true;
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

            // Navigation Properties :
            public ICollection<ServiceType> serviceType { get; set; } = null!;

    }
}
