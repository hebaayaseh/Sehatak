using Sehatak.Application.DTOs.AssignFeaturesWithPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.AssignFeatursToPlan
{
    public interface IPlanFeatureService
    {
        Task<PlanFeatureResponseDto> AssignFeatureAsync(int planId, AssignFeatureToPlanRequestDto requst);
        Task<List<PlanFeatureResponseDto>> GetPlanFeaturesAsync(int planId);
        
    }
}
