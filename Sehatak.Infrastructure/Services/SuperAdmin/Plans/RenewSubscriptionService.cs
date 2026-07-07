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

namespace Sehatak.Infrastructure.Services.SuperAdmin.Plans
{
    public class RenewSubscriptionService : IRenewSubscription 
    {
        private readonly SharedDbContext sharedDbContext;
        public RenewSubscriptionService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public async Task<RenewSubscriptionResponseDto> RenewActiveSubscriptionAsync(int centerId, RenewSubscriptionRequest request)
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

            return new RenewSubscriptionResponseDto
            {
                SubscriptionId = newSubscription.Id,
                CenterId = centerId,
                PlanName = newPlan.Name,
                Amount = newPlan.Price,
                StartDate = newSubscription.StartDate,
                EndDate = newSubscription.EndDate,
                Status = newSubscription.Status,
                Message = "Subscription.RenewedPendingPayment"
            };
        }

        public async Task<RenewSubscriptionResponseDto> RenewExpiredSubscriptionAsync(int centerId, RenewSubscriptionRequest request)
        {
          
            var center = await sharedDbContext.MedicalCenters.FindAsync(centerId);
            if (center == null)
                throw new BusinessException("Center.NotFound");

            var expiredSubscription = await sharedDbContext.CenterSubscriptions
                .FirstOrDefaultAsync(c => c.CenterId == centerId
                                       && c.Status == SubscriptionStatus.Expired);
            if (expiredSubscription == null)
                throw new BusinessException("Subscription.PlanNotFound");

            var newPlan = await sharedDbContext.SubscriptionPlans.FindAsync(request.newPlanId);
            if (newPlan == null || !newPlan.IsActive)
                throw new BusinessException("Subscription.PlanNotFound");

            var newSubscription = new CenterSubscription
            {
                CenterId = centerId,
                PlanId = request.newPlanId,
                Status = SubscriptionStatus.Pending,  
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(newPlan.DurationDays)),
                AmountPaid = newPlan.Price
            };

            await sharedDbContext.CenterSubscriptions.AddAsync(newSubscription);
            await sharedDbContext.SaveChangesAsync();

            return new RenewSubscriptionResponseDto
            {
                SubscriptionId = newSubscription.Id,
                CenterId = centerId,
                PlanName = newPlan.Name,
                Amount = newPlan.Price,
                StartDate = newSubscription.StartDate,
                EndDate = newSubscription.EndDate,
                Status = newSubscription.Status,
                Message = "Subscription.RenewedPendingPayment"
            };

        }
    }
}











  
