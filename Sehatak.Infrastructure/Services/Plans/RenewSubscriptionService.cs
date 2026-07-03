using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.RenewSubscription;
using Sehatak.Application.Interfaces.MedicalCenter;
using Sehatak.Application.Interfaces.RenewSubscription;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using Sehatak.Infrastructure.Services.Features.FeatureService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.Plans
{
    public class RenewSubscriptionService : IRenewSubscription 
    {
        private readonly SharedDbContext sharedDbContext;
        public RenewSubscriptionService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public async Task<bool> RenewSubscriptionAsync(int centerId, RenewSubscriptionRequest request)
        {
            var center = await sharedDbContext.MedicalCenters.FirstOrDefaultAsync(c => c.Id == centerId);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            var currentSubscription = await sharedDbContext.CenterSubscriptions
                .FirstOrDefaultAsync(c => c.CenterId == centerId
                                       && (c.Status == SubscriptionStatus.Active
                                           || c.Status == SubscriptionStatus.Expired));

            if (currentSubscription == null)
                throw new BusinessException("Subscription.PlanNotFound");

           
            var newPlan = await sharedDbContext.SubscriptionPlans.FindAsync(request.newPlanId);
            if (newPlan == null)
                throw new BusinessException("Subscription.PlanNotFound");

           
            currentSubscription.Status = SubscriptionStatus.Expired;

            
            var currentFeatures = await sharedDbContext.CenterFeatures
                .Where(cf => cf.CenterId == centerId)
                .ToListAsync();
            sharedDbContext.CenterFeatures.RemoveRange(currentFeatures);   

           
            var newSubscription = new CenterSubscription
            {
                CenterId = centerId,
                PlanId = request.newPlanId,
                Status = SubscriptionStatus.Active,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(newPlan.DurationDays)),
                AmountPaid = newPlan.Price
            };
            await sharedDbContext.CenterSubscriptions.AddAsync(newSubscription);

            
            var newPlanFeatureIds = await sharedDbContext.PlanFeatures
                .Where(pf => pf.PlanId == request.newPlanId)
                .Select(pf => pf.FeatureId)
                .ToListAsync();

            foreach (var featureId in newPlanFeatureIds)
            {
                await sharedDbContext.CenterFeatures.AddAsync(new CenterFeature
                {
                    CenterId = centerId,
                    FeatureId = featureId,
                    IsEnabled = true
                });
            }

            await sharedDbContext.SaveChangesAsync();
            return true;

        }
    }
}











  
