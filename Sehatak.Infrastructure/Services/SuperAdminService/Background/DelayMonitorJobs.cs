using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sehatak.Domain.Entities.TenantEntities;
using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdminService.Background
{
    public class DelayMonitorJobs : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SubscriptionActivationService> _logger;

        public DelayMonitorJobs(IServiceScopeFactory scopeFactory, ILogger<SubscriptionActivationService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckSlotTime();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task CheckSlotTime()
        {
            using var scope = _scopeFactory.CreateScope();
            var sharedDb = scope.ServiceProvider.GetRequiredService<SharedDbContext>();
            var contextFactory = scope.ServiceProvider.GetRequiredService<TenantDbContextFactory>();

            var activeCenters = await sharedDb.MedicalCenters
                .Where(c => c.CenterStatus == CenterStatus.Active)
                .ToListAsync();

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            foreach (var center in activeCenters)
            {
                try
                {

                    using var db = contextFactory.CreateForCenter(center.Id);

                    var completedToday = await db.Appointments
                        .Where(a => a.appointmentDate == today
                                 && a.appointmentStatus == AppointmentStatus.Completed
                                 && a.actualStartTime.HasValue
                                 && a.actualEndTime.HasValue
                                 && !a.DelayProcessed)
                        .ToListAsync();

                    foreach (var doctorGroup in completedToday.GroupBy(a => a.doctorId))
                    {
                        var doctorId = doctorGroup.Key;
                        var schedule = await db.DoctorSchedules
                            .FirstOrDefaultAsync(s => s.DoctorId == doctorId && s.IsActive);
                        if (schedule == null) continue;

                        double accumulatedDelay = 0;

                        foreach (var appt in doctorGroup)
                        {
                            var actualDuration = (appt.actualEndTime!.Value - appt.actualStartTime!.Value).TotalMinutes;
                            var extraDelay = actualDuration - schedule.SlotDurationMinutes;
                            if (extraDelay > 0)
                                accumulatedDelay += (double)extraDelay;
                            appt.DelayProcessed = true;
                        }

                        while (accumulatedDelay >= schedule.SlotDurationMinutes)
                        {
                            var nextWaiting = await db.Waitlists
                                .Where(w => w.DoctorId == doctorId && w.Status == WaitlistStatus.Waiting)
                                .OrderBy(w => w.CreatedAt)
                                .FirstOrDefaultAsync();

                            if (nextWaiting == null) break;

                            var newAppointment = new Appointment
                            {
                                patientId = nextWaiting.PatientId,
                                doctorId = doctorId,
                                appointmentDate = today,
                                timeSlot = nextWaiting.PreferredTimeSlot.HasValue
                                    ? TimeOnly.FromDateTime(nextWaiting.PreferredTimeSlot.Value)
                                    : (TimeOnly?)null,
                                appointmentStatus = AppointmentStatus.Confirmed,
                            };
                            db.Appointments.Add(newAppointment);
                            await db.SaveChangesAsync();

                            nextWaiting.Status = WaitlistStatus.Entered;
                            nextWaiting.PromotedAppointmentId = newAppointment.Id;

                            accumulatedDelay -= (double)schedule.SlotDurationMinutes;
                        }
                    }

                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Delay monitor failed for center {CenterId}", center.Id);
                    continue;
                }
            }
        }
    }
}
