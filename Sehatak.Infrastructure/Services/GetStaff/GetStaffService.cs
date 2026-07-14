using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.GetStaffDto;
using Sehatak.Application.Interfaces.GetSttafInterFace;
using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.GetStaff
{
    public class GetStaffService : IGetStaff
    {
        private readonly SharedDbContext SharedDbContext;
        private readonly TenantDbContextFactory contextFactory;
        public GetStaffService(TenantDbContextFactory contextFactory,SharedDbContext sharedDbContext)
        {
            this.SharedDbContext = sharedDbContext;
            this.contextFactory = contextFactory;
        }

        public async Task<List<GetDoctorsResponseDto>> GetDoctorsAsync(int centerId)
        {
            var center = await SharedDbContext.MedicalCenters
                 .FirstOrDefaultAsync(c => c.Id == centerId && c.CenterStatus == CenterStatus.Active);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            using var db = contextFactory.CreateForCenter(centerId);

            return await db.Departments
                .Include(d => d.Doctors)
                  .ThenInclude(u => u.user)
                .Select(p => new GetDoctorsResponseDto
                {
                    DepartmentId = p.Id,
                    DepartmentName = p.Name,
                    DepartmentDescription = p.Description,
                    DepartmentImageUrl = p.ImageUrl,
                    Doctors = p.Doctors.Select(a=>new DoctorSummaryDto
                    {
                        DoctorId = a.Id,
                        DoctorName = a.user.firstName+" "+a.user.lastName,
                        Specialization = a.Specialization,
                        ProfileImageUrl = a.user.ProfileImageUrl,
                        OnlineEnabled = a.OnlineEnabled
                    } ).ToList()
                }).ToListAsync();

        }

        public async Task<List<GetLapTechnicalDto>> GetLapTechnicalsAsync(int centerId)
        {
            var center = await SharedDbContext.MedicalCenters
                .FirstOrDefaultAsync(c => c.Id == centerId && c.CenterStatus == CenterStatus.Active);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            using var db = contextFactory.CreateForCenter(centerId);

            return await db.Users
                .Where(u => u.role == userRole.LabTechnician && u.isActive)
                .Select(r => new GetLapTechnicalDto
                {

                    LapTechnicalId = r.Id,
                    LapTechnicalName = r.firstName + " " + r.lastName
                }).ToListAsync();
        }

        public async Task<List<GetReceptionistResponseDto>> GetReceptionistsAsync(int centerId)
        {
            var center = await SharedDbContext.MedicalCenters
               .FindAsync(centerId);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            using var db = contextFactory.CreateForCenter(centerId);

            return await db.Users
                .Where(u => u.role == userRole.Receptionist && u.isActive)
                .Select(r=>new GetReceptionistResponseDto { 
                
                    ReceptionistId = r.Id,
                    ReceptionistName = r.firstName+" "+r.lastName
                }) .ToListAsync();
        }
    }
}
