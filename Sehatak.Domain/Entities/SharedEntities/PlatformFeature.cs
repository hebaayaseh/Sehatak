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

        // اسم الفيتشر مثل "LabAI", "WhatsappNotif", "StaffChat", "OnlineConsultation"
        public string NameOfFeature { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation Properties
        public ICollection<PlanFeature> PlanFeatures { get; set; } = new List<PlanFeature>();
        public ICollection<CenterFeature> CenterFeatures { get; set; } = new List<CenterFeature>();
    }
}
