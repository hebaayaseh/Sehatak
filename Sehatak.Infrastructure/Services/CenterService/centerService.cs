using Sehatak.Application.DTOs.CreateCenterRequestDto;
using Sehatak.Application.Interfaces.MedicalCenter;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.CenterService
{
    public class centerService : ICenterService
    {
        private readonly SharedDbContext sharedDbContext;
        private readonly TenantDbContext tenantDbContext;
        public centerService(SharedDbContext sharedDbContext , TenantDbContext tenantDbContext)
        {
            this.sharedDbContext = sharedDbContext;
            this.tenantDbContext = tenantDbContext;
        }

        public Task<CenterResponseDto> CreateCenterAsync(createCenterRequestDto request)
        {
            
        }
    }
}
