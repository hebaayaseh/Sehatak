using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.AddDoctorDailyHour;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.AddDoctorDailyHours;
using Sehatak.Domain.Entities.TenantEntities;
using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.PostponeEnums;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Services.Implement;

namespace Sehatak.Infrastructure.Services.AddStaff
{
    public class AddDoctorDailyHoursService : IAddDoctorDailyHours
    {
        private readonly SharedDbContext sharedDbContext;
        private readonly TenantDbContextFactory contextFactory;
        public AddDoctorDailyHoursService(SharedDbContext sharedDbContext, TenantDbContextFactory contextFactory)
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
        public async Task<UpdateDoctorDailyHoursResponse> UpdateDoctorDailyHoursAsync(
    int centerId, int userId, int doctorId, UpdateDoctorDailyHousrRequest request)
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
                .Include(d => d.user)
                .FirstOrDefaultAsync(d => d.Id == doctorId && d.user.isActive);
            if (doctor == null)
                throw new BusinessException("Doctor.NotFound");

            var doctorSchedual = await db.DoctorSchedules
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId
                    && d.Id == request.schedualId
                    && d.IsActive);
            if (doctorSchedual == null)
                throw new BusinessException("General.NotFound");


            var hasConflict = await db.DoctorSchedules
                .AnyAsync(s => s.DoctorId == doctorId
                    && s.Id != doctorSchedual.Id
                    && s.IsActive
                    && s.DayOfWeek == request.DayOfWeek
                    && request.StartTime < s.EndTime
                    && request.EndTime > s.StartTime);
            if (hasConflict)
                throw new BusinessException("Schedule.Conflict");


            var affectedAppointments = await db.Appointments
                .Include(a => a.Patient)
                .ThenInclude(p => p.user)
                .Where(a => a.doctorId == doctorId
                    && a.appointmentStatus == AppointmentStatus.Confirmed
                    && a.appointmentDate.DayOfWeek == doctorSchedual.DayOfWeek
                    && a.timeSlot.HasValue
                    && a.timeSlot.Value >= doctorSchedual.StartTime
                    && a.timeSlot.Value < doctorSchedual.EndTime)
                .OrderBy(a => a.appointmentDate)
                .ToListAsync();

            foreach (var appointment in affectedAppointments)
            {
                appointment.appointmentStatus = AppointmentStatus.Postponed;
                db.PostponedServices.Add(new PostponedService
                {
                    PatientId = appointment.patientId,
                    ReceptionistId = userId, 
                    Type = PostponeType.DoctorAppointment,
                    AppointmentId = appointment.Id,
                    Reason = "تعديل جدول دوام الطبيب من قبل الإدارة",
                    Status = PostponeStatus.Active,
                });
                db.Notifications.Add(new Notification
                {
                    UserId = (int)appointment.Patient.userId,
                    Message = "تم تغيير مواعيد دوام الطبيب، يرجى إعادة جدولة الموعد في أقرب وقت",
                    CreatedAt = DateTime.UtcNow,
                    Type = NotificationType.Cancellation,
                    IsRead = false
                });
                //await NotifyPatientPostponeAsync(appointment.Patient.user);
            }


            doctorSchedual.IsActive = false;

            var newSchedule = new Doctorschedule
            {
                DoctorId = doctorId,
                DayOfWeek = request.DayOfWeek,
                SlotDurationMinutes = request.SlotDurationMinutes,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };
            await db.DoctorSchedules.AddAsync(newSchedule);

            await db.SaveChangesAsync();

            return new UpdateDoctorDailyHoursResponse
            {
                DoctorId = doctorId,
                DayOfWeek = newSchedule.DayOfWeek,
                SlotDurationMinutes = request.SlotDurationMinutes,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };
        }

        //private async Task NotifyPatientPostponeAsync(User user)
        //{
        //    await notificationService.CreateAsync(
        //        user.Id,
        //        NotificationType.Cancellation,
        //        "تم تعديل جدول الدكتور، وموعدك يحتاج لإعادة جدولة. يرجى مراجعة التطبيق لاختيار موعد جديد.");

        //    if (!string.IsNullOrWhiteSpace(user.phoneNumber))
        //        await whatsAppService.SendMessageAsync(user.phoneNumber,
        //            "تم تعديل جدول الدكتور، وموعدك يحتاج لإعادة جدولة. يرجى مراجعة التطبيق لاختيار موعد جديد.");
        //}
    }
}
