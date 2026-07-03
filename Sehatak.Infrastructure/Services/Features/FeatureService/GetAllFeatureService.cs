using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.FeatureDto;
using Sehatak.Application.Interfaces.Features;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.Features.FeatureService
{
    public class GetAllFeatureService : IGetAllFeature
    {
        private readonly SharedDbContext sharedDbContext;
        public GetAllFeatureService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public async Task<List<FeatureResponseDto>> GetAllFeatureAsync()
        {
            var listOfFeature = await sharedDbContext.PlatformFeatures
                .Select(f => new FeatureResponseDto
                {
                    Id = f.Id,
                    Name = f.NameOfFeature,
                    Description = f.Description
                })
                .ToListAsync();

            return listOfFeature;
        }
    }
}
