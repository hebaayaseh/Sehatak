using Sehatak.Application.DTOs.FeatureDto;
using Sehatak.Application.Interfaces.Features;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdmin.Features.FeatureService
{
    public class featureService : IFeatureService
    {
        private readonly SharedDbContext sharedDbContext;
        public featureService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public async Task<FeatureResponseDto> AddFeatureAsync(CreateFeatureRequestDto requestDto)
        {
            var feature = new PlatformFeature
            {
                NameOfFeature = requestDto.Name,
                Description = requestDto.Description,
            };
            await sharedDbContext.PlatformFeatures.AddAsync(feature);
            await sharedDbContext.SaveChangesAsync();

            return new FeatureResponseDto {
                Id = feature.Id,
                Name = feature.NameOfFeature,
                Description = feature.Description
            };
        }

        
    }
}
