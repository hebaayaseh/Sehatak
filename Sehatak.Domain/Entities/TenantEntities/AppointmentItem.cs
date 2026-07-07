using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Entities.TenantEntities
{
    public class AppointmentItem
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int ServicePriceId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        // Navigation Properties
        public Appointment Appointment { get; set; } = null!;
        public ServicePrice ServicePrice { get; set; } = null!;
    }
}
