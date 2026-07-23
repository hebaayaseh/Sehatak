using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.AppointmentDto;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.ApointmentInterface;
using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.CalculateSlot;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Sehatak.Infrastructure.Services.AppointmentService
{
    public class AvailableDoctorSlotService : IAppointment
    {
        private readonly SharedDbContext sharedDbContext;
        private readonly TenantDbContextFactory contextFactory;
        private readonly GenerateTheoreticalSlots generateTheoreticalSlots;
        public AvailableDoctorSlotService(SharedDbContext sharedDbContext , TenantDbContextFactory contextFactory , GenerateTheoreticalSlots generateTheoreticalSlot)
        {
            this.sharedDbContext = sharedDbContext;
            this.contextFactory = contextFactory;
            this.generateTheoreticalSlots = generateTheoreticalSlot;
        }

        public async Task<AvailableDoctorSlot> GetAvailableDoctorSlot(int centerId, int doctorId, DateOnly date)
        {
            var center = await sharedDbContext.MedicalCenters
               .FirstOrDefaultAsync(c => c.Id == centerId && c.CenterStatus == CenterStatus.Active);

            if (center == null)
                throw new BusinessException("Center.NotFound");


            using var db = contextFactory.CreateForCenter(centerId);

            var doctor = await db.Doctors
                .Include(u => u.user)
                .Where(d => d.Id == doctorId 
                && d.user.isActive)
                .FirstOrDefaultAsync();

            if (doctor == null)
                throw new BusinessException("Doctor.NotFound");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var isDayBlocked = await db.DoctorBlockedDays
                 .AnyAsync(bd => bd.doctorId == doctorId && bd.isBlocked && bd.date == date);

            if (isDayBlocked)
                throw new BusinessException("Doctor.DayBlocked");


            var schedule = await db.DoctorSchedules
                .Include(d => d.doctor)
                .Where(d => d.DoctorId == doctorId
                       && d.IsActive
                       && d.DayOfWeek == date.DayOfWeek)
                .FirstOrDefaultAsync();

            if (schedule == null)
                throw new BusinessException("Doctor.NotFound");

            var theoreticalSlots = generateTheoreticalSlots.GenerateTheoreticalSlot(schedule.StartTime, schedule.EndTime, (int)schedule.SlotDurationMinutes);

            var bookedSlots = await db.Appointments
                .Where(a => a.doctorId == doctorId
                       && a.appointmentStatus == AppointmentStatus.Confirmed
                       && a.appointmentDate == date)
                       .Select(a => a.timeSlot)
                       .ToListAsync();


            var availableSlots = theoreticalSlots
                .Where(slot => slot.HasValue)
                .Select(slot => slot!.Value)
                .Except(bookedSlots.Where(b => b.HasValue).Select(b => b!.Value))
                .OrderBy(s => s)
                .ToList();



            return new AvailableDoctorSlot
            {
                doctorId = doctorId,
                doctorName = $"{doctor.user.firstName} {doctor.user.lastName}",
                DayOfWeek = schedule.DayOfWeek,
                date = date,
                dateAvailable = availableSlots
            };

        }
    }
}
