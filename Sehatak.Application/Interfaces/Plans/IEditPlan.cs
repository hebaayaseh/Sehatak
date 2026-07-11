using Sehatak.Application.DTOs.Plans;
using Sehatak.Application.DTOs.PlansDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.Plans
{
    public interface IEditPlan
    {
        Task<EditRespondeDto> EditPlanAsync(int planId,EditPalnRequestDto request);
    }
}
