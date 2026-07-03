using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.FeatureCenterDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces
{
    public interface IActiveFeature
    {
        Task<bool> ActiveFeaturAsync(int centerId, ActiveFetureRequest request);
    }
}
