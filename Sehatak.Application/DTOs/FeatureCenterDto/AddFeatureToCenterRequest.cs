using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.FeatureCenterDto
{
    public class AddFeatureToCenterRequest
    {
        public int featureId { get; set; }
        public string featureName { get; set; }
    }
}
