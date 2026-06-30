using Sehatak.Application.DTOs.Plans;
using Sehatak.Domain.Entities.SharedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.Plans
{
    public interface ISubscriptionPlan
    {
        Task<SubscriptionPlanResponseDto> AddSubscriptionPlan(SubscriptionPlanRequestDto request);
    }

}
