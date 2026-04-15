using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Entities.SharedEntities
{
    public class PlatformFeature
    {
        public int Id { get; set; }
        public string NameOfFeature { get; set; } = string.Empty;// "LabAI", "WhatsappNotif", "StaffChat"
        // Navigation Properties :
        public ICollection<CenterFeature> CenterFeatures { get; set; } = new List<CenterFeature>();
        public SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
