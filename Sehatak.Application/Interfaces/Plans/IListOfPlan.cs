using Sehatak.Application.DTOs.PlansDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.Plans
{
    public interface IListOfPlan
    {
        Task<ListOfPlanResponseDto> ListOfPlanAsync();
    }
}
