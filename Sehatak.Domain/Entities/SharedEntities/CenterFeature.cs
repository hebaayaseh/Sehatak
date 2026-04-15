using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Entities.SharedEntities
{
    public class CenterFeature
    {
        public int CenterId { get; set; }
        public int FeatureId { get; set; }
        public bool IsEnabled { get; set; } = true;
        // Navigation Properties :
        public MedicalCenter Center { get; set; } = null!;
        public PlatformFeature Feature { get; set; } = null!;
    }
}
