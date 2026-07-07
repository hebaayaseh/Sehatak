using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.CentersDto;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.Centers;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdmin.CenterService
{
    public class SpasificCenterService : ISpasificCenter
    {
        private readonly SharedDbContext sharedDbContext;
        public SpasificCenterService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }
        public async Task<SpasificCenterResponseDto> GetSpasificCenterById(int centerId)
        {
            var center = await sharedDbContext.MedicalCenters
                .FirstOrDefaultAsync(c => c.Id == centerId);
            if (center == null)
                throw new BusinessException("Center.NotFound");

            var subscription = await sharedDbContext.CenterSubscriptions
                .Include(c => c.Plan)
                .FirstOrDefaultAsync(c => c.CenterId == centerId && c.Status == SubscriptionStatus.Active);

            if (subscription == null)
                throw new BusinessException("Center.SubscriptionExpired");

            var features = await sharedDbContext.PlanFeatures
                .Where(p => p.PlanId == subscription.PlanId)
                .Select(f => f.Feature.NameOfFeature)
                .ToListAsync();

            return new SpasificCenterResponseDto
            {
                Id = center.Id,
                Name = center.Name,
                UniqueUrl = center.UniqueUrl,
                Phone = center.Phone,
                Address = center.Address,
                LogoUrl = center.LogoUrl,
                AddedBySuperAdminId = center.AddedBySuperAdminId,
                AdminWhatsappNumber = center.AdminWhatsappNumber,
                PlanName = subscription.Plan.Name,
                FeatureNames = features
            };


        }
    }
}
