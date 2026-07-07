using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.FeatureCenterDto;
using Sehatak.Application.Interfaces.RemoveFeatureFromCenter;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdminService.Features.RemoveFeatureFromCenter
{
    public class RemoveFeatureFromCenterService : IRemoveFeatureFromCenter
    {
        private readonly SharedDbContext sharedDbContext;
        public RemoveFeatureFromCenterService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }
        public async Task<bool> RemoveFeatureFromCenterAsync(int centerId, RemoveFeatureFromCenterRequest request)
        {
            
            var center = await sharedDbContext.MedicalCenters.FindAsync(centerId);

            if (center == null)
            {
                throw new BusinessException("Center.NotFound");
            }

            var feature = await sharedDbContext.PlatformFeatures
                .FirstOrDefaultAsync(f=> f.Id == request.featureId);
            if (feature == null)
            {
                throw new BusinessException("General.NotFound");
            }

            var centerFeature = await sharedDbContext.CenterFeatures
                .FirstOrDefaultAsync(cf => cf.CenterId == centerId 
            && cf.FeatureId == request.featureId);
            if (centerFeature == null)
            {
                throw new BusinessException("General.NotFound");
            }

            centerFeature.IsEnabled = false;

            await sharedDbContext.SaveChangesAsync();

            return true;

        }
    }
}
