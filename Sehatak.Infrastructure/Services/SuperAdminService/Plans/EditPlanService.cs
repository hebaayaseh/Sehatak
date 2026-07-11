using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.Plans;
using Sehatak.Application.DTOs.PlansDto;
using Sehatak.Application.Interfaces.Plans;
using Sehatak.Domain.Entities;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdminService.Plans
{
    public class EditPlanService : IEditPlan
    {
        private readonly SharedDbContext sharedDbContext;
        public EditPlanService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public async Task<EditRespondeDto> EditPlanAsync(int planId,EditPalnRequestDto request)
        {
            var Plan = await sharedDbContext.SubscriptionPlans
                .FirstOrDefaultAsync(p => p.Id == planId);

            if (Plan == null)
                throw new BusinessException("Subscription.PlanNotFound");


            if (request.price != null)
                Plan.Price = request.price.Value;

            if (request.name != null)
                Plan.Name = request.name;

            if (request.DurationDays != null)
                Plan.DurationDays = request.DurationDays.Value;

            await sharedDbContext.SaveChangesAsync();

            return new EditRespondeDto
            {
                Id = planId,
                price = Plan.Price,
                name = Plan.Name,
                DurationDays = Plan.DurationDays
            };

        }
    }
}
