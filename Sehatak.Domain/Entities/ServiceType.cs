using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Entities
{
    public class ServiceType
    {
        public int Id { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        // Navigation Properties :
        public ServicePrice servicePrice { get; set; } = null!;

    }
}
