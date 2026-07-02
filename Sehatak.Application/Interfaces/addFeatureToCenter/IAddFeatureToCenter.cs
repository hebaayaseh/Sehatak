using Sehatak.Application.DTOs.FeatureCenterDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.AddFeatureToCenter
{
    public interface IAddFeatureToCenter
    {
        Task<bool> AddFeatureToCenterAsync(int centerId,AddFeatureToCenterRequest request);
    }
}
