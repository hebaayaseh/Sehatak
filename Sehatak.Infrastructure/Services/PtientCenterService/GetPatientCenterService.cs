using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.PatientCenter;
using Sehatak.Application.Interfaces.IPatientCenter;
using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.PtientCenterService
{
    public class GetPatientCenterService : IGetpatientCenter
    {
        private readonly SharedDbContext SharedDbContext;
        private readonly TenantDbContextFactory contextFactory;
        public GetPatientCenterService(SharedDbContext sharedDbContext, TenantDbContextFactory contextFactory)
        {
            SharedDbContext = sharedDbContext;
            this.contextFactory = contextFactory;
        }

        public async Task<List<GetPatientResponseDto>> GetPatientesAsync(int centerId)
        {
            var center = await SharedDbContext.MedicalCenters
                .FirstOrDefaultAsync(c => c.Id == centerId && c.CenterStatus == CenterStatus.Active);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            using var db = contextFactory.CreateForCenter(centerId);

            return await db.Users
                .Include(u => u.patient)
                .ThenInclude(p => p.appointments)
                .Where(u => u.role == userRole.Patient && u.patient != null)
                .Select(u => new GetPatientResponseDto
                {
                    Id = u.Id,
                    pateintName = u.firstName+" "+u.lastName,
                    BloodType = u.patient.BloodType,
                    Gender = u.patient.Gender,
                    appointments = u.patient.appointments
                    .Select(a=>a.Doctor.user.firstName+""+ a.Doctor.user.lastName)
                    .ToList(),

                }).ToListAsync();
        }
    }
}
