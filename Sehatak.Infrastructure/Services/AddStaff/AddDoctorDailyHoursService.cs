using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.AddDoctorDailyHour;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.AddDoctorDailyHours;
using Sehatak.Domain.Entities.TenantEntities;
using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.AddStaff
{
    public class AddDoctorDailyHoursService : IAddDoctorDailyHours
    {
        private readonly SharedDbContext sharedDbContext;
        private readonly TenantDbContextFactory contextFactory;
        public AddDoctorDailyHoursService(SharedDbContext sharedDbContext , TenantDbContextFactory contextFactory)
        {
            this.sharedDbContext = sharedDbContext;
            this.contextFactory = contextFactory;
        }

        public async Task<AddDoctorDailyHoursResponse> AddDoctorDailyHoursAsync(int centerId, int userId, int doctorId, AddDoctorDailyHoursRequest request)
        {
            var center = await sharedDbContext.MedicalCenters
                .FirstOrDefaultAsync(c => c.Id == centerId && c.CenterStatus == CenterStatus.Active);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            using var db = contextFactory.CreateForCenter(centerId);

            var admin = await db.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.role == userRole.Admin && u.isActive);

            if (admin == null)
                throw new BusinessException("Auth.Forbidden");


            var doctor = await db.Doctors
                .Include(u => u.user)
                .FirstOrDefaultAsync(d => d.Id == doctorId && d.user.isActive);

            if (doctor == null)
                throw new BusinessException("Doctor.NotFound");

            var hasConflict = await db.DoctorSchedules
              .AnyAsync(s => s.DoctorId == doctorId
                    && s.DayOfWeek == request.DayOfWeek
                    && request.StartTime < s.EndTime
                    && request.EndTime > s.StartTime);
            if (hasConflict)
                throw new BusinessException("Schedule.Conflict");

            var doctorScheduale = new Doctorschedule
            {
                DoctorId = doctorId,
                DayOfWeek = request.DayOfWeek,
                SlotDurationMinutes = request.SlotDurationMinutes,
                StartTime = request.StartTime,
                EndTime = request.EndTime

            };

            await db.DoctorSchedules.AddAsync(doctorScheduale);
            await db.SaveChangesAsync();

            return new AddDoctorDailyHoursResponse
            {
                DoctorId = doctorId,
                DayOfWeek = doctorScheduale.DayOfWeek,
                SlotDurationMinutes = request.SlotDurationMinutes,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };
        }
        
    }
}
