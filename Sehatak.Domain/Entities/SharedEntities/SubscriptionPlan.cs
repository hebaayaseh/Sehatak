
using System.ComponentModel.DataAnnotations;

namespace Sehatak.Domain.Entities.SharedEntities
{
    public class SubscriptionPlan
    {
        
        public int Id { get; set; }
        // Name of plane
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public bool IsActive { get; set; } = true;
        //  Navigation Properties :
        public ICollection<CenterSubscription> CenterSubscriptions { get; set; } = new List<CenterSubscription>();
        public ICollection<PlanFeature> PlanFeatures { get; set; } = new List<PlanFeature>();
    }
}
