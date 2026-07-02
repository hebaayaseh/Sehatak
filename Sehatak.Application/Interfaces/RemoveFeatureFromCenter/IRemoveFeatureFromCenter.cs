using Sehatak.Application.DTOs.FeatureCenterDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.RemoveFeatureFromCenter
{
    public interface IRemoveFeatureFromCenter
    {
        Task<bool> RemoveFeatureFromCenterAsync(int centerId, RemoveFeatureFromCenterRequest request);
    }
}
