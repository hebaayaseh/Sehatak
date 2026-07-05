using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.FeatureCenterDto;
using Sehatak.Application.Interfaces.Features;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.Features.AddFeatureToCenter
{
    public class ActiveFeatureService : IActiveFeature
    {
        private readonly SharedDbContext sharedDbContext;
        public ActiveFeatureService (SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }
        public async Task<bool> ActiveFeaturAsync(int centerId,ActiveFetureRequest request)
        {
            var center = await sharedDbContext.MedicalCenters.FindAsync(centerId);
            if (center == null)
            {
                throw new BusinessException("Center.NotFound");
            }
            var feature = await sharedDbContext.PlatformFeatures.FirstOrDefaultAsync(f => f.Id == request.FetureId);
            if (feature == null)
            {
                throw new BusinessException("General.NotFound");
            }

            var existingCenterFeature = await sharedDbContext.CenterFeatures
                .FirstOrDefaultAsync(cf => cf.CenterId == centerId && cf.FeatureId == request.FetureId && cf.IsEnabled==false);
            if(existingCenterFeature == null)
            {
                throw new BusinessException("General.NotFound");
            }

            existingCenterFeature.IsEnabled = true;
            await sharedDbContext.SaveChangesAsync();

            return true;

        }
    }
}
