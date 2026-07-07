using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.AssignFeaturesWithPlan;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.AssignFeatursToPlan;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdminService.Features.AssignFeatureToPlan
{
    public class PlanFeatureService : IPlanFeatureService
    {
        private readonly SharedDbContext sharedDbContext;
        public PlanFeatureService (SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public async Task<PlanFeatureResponseDto> AssignFeatureAsync(int planId, AssignFeatureToPlanRequestDto request)
        {
            var plan = await sharedDbContext.SubscriptionPlans.FindAsync(planId);
            if (plan == null) 
                throw new BusinessException("Subscription.PlanNotFound");

            var feature = await sharedDbContext.PlatformFeatures.FindAsync(request.featureId);
            if (feature == null)
                throw new BusinessException("General.NotFound");

            var alreadyLinked = await sharedDbContext.PlanFeatures.FirstOrDefaultAsync(p=> p.PlanId == planId && p.FeatureId == request.featureId);
            if(alreadyLinked != null)
                throw new BusinessException("General.NotFound");

            var planFeature = new PlanFeature
            {
                PlanId = planId,
                FeatureId = request.featureId,
            };

            await sharedDbContext.PlanFeatures.AddAsync(planFeature);
            await sharedDbContext.SaveChangesAsync();

            return new PlanFeatureResponseDto
            {
                planId = planId,
                featureId = feature.Id,
                featureName = feature.NameOfFeature
            };

        }

        public async Task<List<PlanFeatureResponseDto>> GetPlanFeaturesAsync(int planId)
        {
            return await sharedDbContext.PlanFeatures
                .Where(pf => pf.PlanId == planId)
                .Select(pf => new PlanFeatureResponseDto
                {
                    planId = pf.PlanId,
                    featureId = pf.FeatureId,
                    featureName = pf.Feature.NameOfFeature
                })
                .ToListAsync();
        }
    }
}
