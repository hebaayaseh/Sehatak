using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.CentersDto;
using Sehatak.Application.Interfaces.Centers;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.CenterService
{
    public class listOfCentersService : IListOfCenters
    {
        private readonly SharedDbContext sharedDbContext;
        public listOfCentersService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public async Task<List<ListOfCentersResponse>> GetListOfCenters()
        {
            var centers = await sharedDbContext.MedicalCenters
                .Select(c => new ListOfCentersResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    UniqueUrl = c.UniqueUrl,
                    Phone = c.Phone,
                    Address = c.Address,
                    LogoUrl = c.LogoUrl,
                    AddedBySuperAdminId = c.AddedBySuperAdminId,
                    AdminWhatsappNumber = c.AdminWhatsappNumber
                    
                })
                .ToListAsync();
            
            return centers;

        }
    }
}
