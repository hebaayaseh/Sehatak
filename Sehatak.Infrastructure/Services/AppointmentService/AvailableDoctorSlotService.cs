using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.AppointmentDto;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.ApointmentInterface;
using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.AppointmentService
{
    public class AvailableDoctorSlotService : IAppointment
    {
        private readonly SharedDbContext sharedDbContext;
        private readonly TenantDbContextFactory contextFactory;
        public AvailableDoctorSlotService(SharedDbContext sharedDbContext , TenantDbContextFactory contextFactory)
        {
            this.sharedDbContext = sharedDbContext;
            this.contextFactory = contextFactory;
        }

        public async Task<AvailableDoctorSlot> GetAvailableDoctorSlot(int centerId, int doctorId)
        {
            var center = await sharedDbContext.MedicalCenters
                .Where(c => c.Id == centerId && c.CenterStatus == CenterStatus.Active)
                .ToListAsync();

            if (center == null)
                throw new BusinessException("Center.NotFound");

            using var db = contextFactory.CreateForCenter(centerId);

            var doctor = await db.Doctors
                .Include(u => u.user)
                .Where(d => d.Id == doctorId && d.user.isActive)
                .FirstOrDefaultAsync();

            if (doctor == null)
                throw new BusinessException("Doctor.NotFound");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var blockedDay = db.DoctorBlockedDays
                .Where(bd => bd.doctorId == doctorId && bd.isBlocked && bd.date >= today)
                .ToListAsync();

            if (blockedDay != null)
                throw new BusinessException("Doctor.CancelDay");

            var doctorSchedual = await db.DoctorSchedules
                .Include(d => d.doctor)
                .Where(d => d.DoctorId == doctorId
                       && d.IsActive)
                .ToListAsync();

            if (doctorSchedual == null)
                throw new BusinessException("Doctor.NotFound");

            var appointment = await db.Appointments
                .Include(d => d.Doctor)
                .ThenInclude(d=>doctorSchedual)
                .Where(a => a.doctorId == doctorId
                      && a.appointmentStatus == AppointmentStatus.Pending
                      && a.timeSlot <= )
                .ToListAsync();

            
            



        }
    }
}
