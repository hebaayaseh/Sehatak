using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.CentersStatus;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdmin.CenterService
{
    public class CenterStatusService : ICentersStatus
    {
        private readonly SharedDbContext sharedDbContext;
        public CenterStatusService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public async Task<bool> SuspendedCenter(int centerId)
        {
            var center = await sharedDbContext.MedicalCenters.FirstOrDefaultAsync(c=>c.Id == centerId 
            && c.CenterStatus == CenterStatus.Active);
            if(center == null)
                throw new BusinessException("Center.NotFound");

            var subscription = await sharedDbContext.CenterSubscriptions
                .FirstOrDefaultAsync(s => s.Center.Id == centerId 
                && s.Status == SubscriptionStatus.Active);

            if (subscription == null)
                throw new BusinessException("Subscription.PlanNotFound");


            center.CenterStatus = CenterStatus.Suspended;
            subscription.Status = SubscriptionStatus.Cancelled;

            await sharedDbContext.SaveChangesAsync();
            return true;
            
        }
        public async Task<bool> ActiveCenter(int centerId)
        {
            var center = await sharedDbContext.MedicalCenters.FirstOrDefaultAsync(c => c.Id == centerId 
            && c.CenterStatus == CenterStatus.Suspended);
            if (center == null)
                throw new BusinessException("Center.NotFound");

            var subscription = await sharedDbContext.CenterSubscriptions
                .FirstOrDefaultAsync(s => s.Center.Id == centerId 
                && s.Status == SubscriptionStatus.Cancelled);

            if (subscription == null)
                throw new BusinessException("Subscription.PlanNotFound");

            if (subscription.EndDate < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new BusinessException("Center.SubscriptionExpired");

            center.CenterStatus = CenterStatus.Active;
            subscription.Status = SubscriptionStatus.Active;
            await sharedDbContext.SaveChangesAsync();

            return true;
        }

        
    }
}
