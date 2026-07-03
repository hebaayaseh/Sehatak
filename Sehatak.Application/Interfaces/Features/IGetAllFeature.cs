using Sehatak.Application.DTOs.FeatureDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.Features
{
    public interface IGetAllFeature
    {
        Task<List<FeatureResponseDto>> GetAllFeatureAsync();
    }
}
