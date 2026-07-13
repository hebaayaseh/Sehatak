using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.PlansDto;
using Sehatak.Application.Interfaces.Plans;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdminService.Plans
{
    public class ListOfPlanService : IListOfPlan
    {
        private readonly SharedDbContext sharedDbContext;
        public ListOfPlanService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        
    }
}
