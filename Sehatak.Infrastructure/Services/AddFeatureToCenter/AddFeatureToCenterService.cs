using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.FeatureCenterDto;
using Sehatak.Application.Interfaces.AddFeatureToCenter;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.AddFeatureToCenter
{
    public class AddFeatureToCenterService : IAddFeatureToCenter
    {
        private readonly SharedDbContext sharedDbContext;
        public AddFeatureToCenterService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }
        public async Task<bool> AddFeatureToCenterAsync(int centerId, AddFeatureToCenterRequest request)
        {
            var center = await sharedDbContext.MedicalCenters.FindAsync(centerId);
            if (center == null)
            {
                throw new BusinessException("Center.NotFound");
            }

            var featureExists = await sharedDbContext.PlatformFeatures
                .AnyAsync(f => f.Id == request.featureId);
            if (!featureExists)
                throw new BusinessException("General.NotFound");

            var alreadyAdded = await sharedDbContext.CenterFeatures
                .AnyAsync(cf => cf.CenterId == centerId && cf.FeatureId == request.featureId);
            if (alreadyAdded)
                throw new BusinessException("General.NotFound");

            var feature = new CenterFeature
            {
                CenterId = centerId,
                FeatureId = request.featureId,
                IsEnabled = true
            };

            await sharedDbContext.CenterFeatures.AddAsync(feature);
            await sharedDbContext.SaveChangesAsync();

            return true;

        }
    }
}
