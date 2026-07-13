using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.DepartmentDto;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.FeatureCenterDto;
using Sehatak.Application.Interfaces.DepartmentInterface;
using Sehatak.Application.Interfaces.RemoveFeatureFromCenter;
using Sehatak.Domain.Enums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.DepartmentService
{
    public class RemoveStaffFromCenter : IRemoveStaff
    {
        public TenantDbContextFactory contextFactory;
        public SharedDbContext SharedDbContext;
        public RemoveStaffFromCenter(TenantDbContextFactory contextFactory, SharedDbContext sharedDbContext)
        {
            this.contextFactory = contextFactory;
            this.SharedDbContext = sharedDbContext;
        }

        public async Task<bool> ActiveStaffAsync(int centerId, RemoveStaffRequestDto request)
        {
            var center = await SharedDbContext.MedicalCenters
              .FindAsync(centerId);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            using var db = contextFactory.CreateForCenter(centerId);
            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == request.userId);

            if (user == null)
                throw new BusinessException("General.NotFound");
            if (user.role == userRole.Patient)
                throw new BusinessException("Auth.Forbidden");

            user.isActive = true;
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveStaffAsync(int centerId, RemoveStaffRequestDto request)
        {
            var center = await SharedDbContext.MedicalCenters
                .FindAsync(centerId);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            using var db = contextFactory.CreateForCenter(centerId);
            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == request.userId);

            if (user == null)
                throw new BusinessException("General.NotFound");
            if(user.role == userRole.Patient)
               throw new BusinessException("Auth.Forbidden");  

            user.isActive = false;   
            await db.SaveChangesAsync();

            return true;
        }
    }
}
