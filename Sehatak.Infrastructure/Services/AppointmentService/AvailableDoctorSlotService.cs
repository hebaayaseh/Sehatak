using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.AppointmentDto;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.ApointmentInterface;
using Sehatak.Domain.Entities.TenantEntities;
using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.CalculateSlot;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<BookAppointmentRespesponse> BookAppointmentAsync(int centerId , int doctorId ,int userId , BookAppointmentRequest request)
        {
            var center = await sharedDbContext.MedicalCenters
                .FirstOrDefaultAsync(c => c.Id == centerId
                                     && c.CenterStatus == CenterStatus.Active);

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


            var isDayBlocked = await db.DoctorBlockedDays
                 .AnyAsync(bd => bd.doctorId == doctorId && bd.isBlocked
                           && bd.date == request.dateOnly);

            if (isDayBlocked)
                throw new BusinessException("Doctor.DayBlocked");

            var schedule = await db.DoctorSchedules
               .Include(d => d.doctor)
               .Where(d => d.DoctorId == doctorId
                      && d.IsActive
                      && d.DayOfWeek == request.dateOnly.DayOfWeek)
               .FirstOrDefaultAsync();

            if (schedule == null)
                throw new BusinessException("Doctor.NotFound");

            var theoreticalSlots = generateTheoreticalSlots.GenerateTheoreticalSlot(schedule.StartTime, schedule.EndTime, (int)schedule.SlotDurationMinutes);

            var bookedSlots = await db.Appointments
                .Where(a => a.doctorId == doctorId
                       && a.appointmentStatus == AppointmentStatus.Confirmed
                       && a.appointmentDate == request.dateOnly)
                      .Select(a => a.timeSlot)
                      .ToListAsync();


            var availableSlots = theoreticalSlots
                .Where(slot => slot.HasValue)
                .Select(slot => slot!.Value)
                .Except(bookedSlots.Where(b => b.HasValue).Select(b => b!.Value))
                .OrderBy(s => s)
                .ToList();

            var patient = await db.Patients
                .Include(u => u.user)
                .FirstOrDefaultAsync(u => u.userId == userId && u.user.isActive);

            if (patient == null)
                throw new BusinessException("Auth.Forbidden");

            if (!availableSlots.Contains(request.timeSlot))
            {
                if (availableSlots.Any())
                {
                    return new BookAppointmentRespesponse
                    {
                        Message = "هذا الموعد محجوز من قبل , يمكنك اختيار موعد اخر !",
                        Success = false,
                        AlternativeSlots = availableSlots
                    };

                }
                else
                {

                    db.Waitlists.Add(new Waitlist
                    {
                        PatientId = patient.patientId,
                        DoctorId = doctorId,
                        PreferredDate = request.dateOnly,
                        PreferredTimeSlot = request.timeSlot,
                        Status = WaitlistStatus.Waiting,
                        CreatedAt = DateTime.UtcNow
                    });
                    await db.SaveChangesAsync();
                    return new BookAppointmentRespesponse
                    {
                        Message = "اليوم ممتلئ بالكامل , تم وضعك في قائمة الانتظار في حال توفر موعد سيتم اخبارك",
                        Success = false,
                        AlternativeSlots = null
                    };
                }
            }


            await db.Appointments.AddAsync(
                new Appointment
                {
                    patientId = patient.patientId,
                    timeSlot = request.timeSlot,
                    appointmentDate = request.dateOnly,
                    appointmentStatus = AppointmentStatus.Confirmed,
                    doctorId = doctorId,
                    IsEmergency = false,
                    updateAt = DateTime.UtcNow,
                    createdAt = DateTime.UtcNow

                });

            await db.SaveChangesAsync();

            return new BookAppointmentRespesponse
            {
                Message = "تم حجز الموعد بنجاح",
                Success = true,
                AlternativeSlots = null
            };


        }

        
    }
}
