using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Entities.SharedEntities
{
    public class PlanFeature
    {
        public int PlanId { get; set; }
        public int FeatureId { get; set; }

        // Navigation Properties
        public SubscriptionPlan Plan { get; set; } = null!;
        public PlatformFeature Feature { get; set; } = null!;
    }
}
