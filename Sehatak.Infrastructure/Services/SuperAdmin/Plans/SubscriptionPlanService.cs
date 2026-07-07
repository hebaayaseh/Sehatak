using Sehatak.Application.DTOs.Plans;
using Sehatak.Application.Interfaces.Plans;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdmin.Plans
{
    public class SubscriptionPlanService : ISubscriptionPlan
    {
        private readonly SharedDbContext sharedDbContext;
        public SubscriptionPlanService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public async Task<SubscriptionPlanResponseDto> AddSubscriptionPlan(SubscriptionPlanRequestDto request)
        {
            var supscriptionPlan = new SubscriptionPlan
            {
                Name = request.Name,
                DurationDays = request.DurationDays,
                Price = request.Price,
                
            };
            await sharedDbContext.SubscriptionPlans.AddAsync(supscriptionPlan);
            await sharedDbContext.SaveChangesAsync();

            return new SubscriptionPlanResponseDto
            {
                Id = supscriptionPlan.Id,
                Name = supscriptionPlan.Name,
                DurationDays = supscriptionPlan.DurationDays,
                Price = supscriptionPlan.Price
            };
        }
    }
}
