using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.RenewSubscription;
using Sehatak.Application.Interfaces.MedicalCenter;
using Sehatak.Application.Interfaces.RenewSubscription;
using Sehatak.Domain.Entities;
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

        public async Task<bool> RenewActiveSubscriptionAsync(int centerId, RenewSubscriptionRequest request)
        {
            var center = await sharedDbContext.MedicalCenters.FindAsync(centerId);
            if (center == null)
                throw new BusinessException("Center.NotFound");

            var currentSubscription = await sharedDbContext.CenterSubscriptions
                .FirstOrDefaultAsync(c => c.CenterId == centerId
                                       && c.Status == SubscriptionStatus.Active);

            if (currentSubscription == null)
                throw new BusinessException("Subscription.PlanNotFound");

            var pendingExists = await sharedDbContext.CenterSubscriptions
                .AnyAsync(c => c.CenterId == centerId && c.Status == SubscriptionStatus.Pending);
            if (pendingExists)
                throw new BusinessException("Subscription.Renewed"); 


            var newPlan = await sharedDbContext.SubscriptionPlans.FindAsync(request.newPlanId);
            if (newPlan == null)
                throw new BusinessException("Subscription.PlanNotFound");

            
            var newSubscription = new CenterSubscription
            {
                CenterId = centerId,
                PlanId = request.newPlanId,
                Status = SubscriptionStatus.Pending,
                StartDate = currentSubscription.EndDate,
                EndDate = currentSubscription.EndDate.AddDays(newPlan.DurationDays),
                AmountPaid = newPlan.Price,
                
            };

            
            
            await sharedDbContext.CenterSubscriptions.AddAsync(newSubscription);
            await sharedDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RenewExpiredSubscriptionAsync(int centerId, RenewSubscriptionRequest request)
        {
            var center = await sharedDbContext.MedicalCenters.FindAsync(centerId);
            if (center == null)
                throw new BusinessException("Center.NotFound");

            var currentSubscription = await sharedDbContext.CenterSubscriptions
                .FirstOrDefaultAsync(c => c.CenterId == centerId && 
                c.PlanId == request.oldPlanId&& c.Status == SubscriptionStatus.Expired);

            if (currentSubscription == null)
                throw new BusinessException("Subscription.PlanNotFound");

            var newPlan = await sharedDbContext.SubscriptionPlans.FindAsync(request.newPlanId);
            if (newPlan == null)
                throw new BusinessException("Subscription.PlanNotFound");

            if (request.newPlanId != request.oldPlanId)
            {
                var oldFeatures = await sharedDbContext.CenterFeatures
                    .Where(c => c.CenterId == centerId)
                    .ToListAsync();

                sharedDbContext.CenterFeatures.RemoveRange(oldFeatures);

                var newFeatures = await sharedDbContext.PlanFeatures
                    .Where(p => p.PlanId == request.newPlanId)
                    .Select(p => p.FeatureId)
                    .ToListAsync();

                foreach (var featureId in newFeatures)
                {
                    sharedDbContext.CenterFeatures.Add(new CenterFeature
                    {
                        CenterId = centerId,
                        FeatureId = featureId,
                        IsEnabled = true
                    });
                }
            }

            var newSubscription = new CenterSubscription
            {
                CenterId = centerId,
                PlanId = request.newPlanId,
                Status = SubscriptionStatus.Active,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(newPlan.DurationDays)),
                AmountPaid = newPlan.Price,
            };
            
            if (center.CenterStatus == CenterStatus.Suspended)
                center.CenterStatus = CenterStatus.Active;

            await sharedDbContext.CenterSubscriptions.AddAsync(newSubscription);
            await sharedDbContext.SaveChangesAsync();
            return true;


        }
    }
}











  
