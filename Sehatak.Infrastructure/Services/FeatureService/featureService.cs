using Sehatak.Application.DTOs.FeatureDto;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.FeatureService
{
    public class featureService
    {
        private readonly SharedDbContext sharedDbContext;
        public featureService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public async Task addFeature(FeatureDto featureDto)
        {
            var feature = new PlatformFeature{
                NameOfFeature = featureDto.Name,
                Description = featureDto.Description,
            };
            await sharedDbContext.PlatformFeatures.AddAsync(feature);
            await sharedDbContext.SaveChangesAsync();

        }
    }
}
