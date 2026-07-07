using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sehatak.Application.Interfaces.IEmail;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdminService.Background
{
    public class SubscriptionActivationService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SubscriptionActivationService> _logger;

        public SubscriptionActivationService( IServiceScopeFactory scopeFactory,ILogger<SubscriptionActivationService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckAndActivateSubscriptions();
                    await SendRenewalReminders();  
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in SubscriptionActivationService");
                }
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task CheckAndActivateSubscriptions()
        {
            using var scope = _scopeFactory.CreateScope();
            var sharedDb = scope.ServiceProvider.GetRequiredService<SharedDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var expiredSubscriptions = await sharedDb.CenterSubscriptions
                .Include(s => s.Center)
                .Where(s => s.Status == SubscriptionStatus.Active && s.EndDate <= today)
                .ToListAsync();

            foreach (var expired in expiredSubscriptions)
            {
                expired.Status = SubscriptionStatus.Expired;

                var pending = await sharedDb.CenterSubscriptions
                    .FirstOrDefaultAsync(s => s.CenterId == expired.CenterId
                                           && s.Status == SubscriptionStatus.Pending);

                if (pending != null)
                {
                    pending.Status = SubscriptionStatus.Active;

                    var oldFeatures = await sharedDb.CenterFeatures
                        .Where(cf => cf.CenterId == expired.CenterId)
                        .ToListAsync();
                    sharedDb.CenterFeatures.RemoveRange(oldFeatures);

                    var newPlanFeatureIds = await sharedDb.PlanFeatures
                        .Where(pf => pf.PlanId == pending.PlanId)
                        .Select(pf => pf.FeatureId)
                        .ToListAsync();

                    foreach (var featureId in newPlanFeatureIds)
                    {
                        sharedDb.CenterFeatures.Add(new CenterFeature
                        {
                            CenterId = expired.CenterId,
                            FeatureId = featureId,
                            IsEnabled = true
                        });
                    }

                    if (!string.IsNullOrEmpty(expired.Center?.AdminWhatsappNumber))
                    {
                        await emailService.SendPaymentConfirmedAsync(
                            expired.Center.AdminWhatsappNumber,
                            expired.Center.Name,
                            pending.AmountPaid
                        );
                    }

                    _logger.LogInformation(
                        "Subscription activated for center {CenterId}", expired.CenterId);
                }
                else
                {
                    var center = await sharedDb.MedicalCenters.FindAsync(expired.CenterId);
                    if (center != null)
                    {
                        center.CenterStatus = CenterStatus.Suspended;

                        // ← إيميل إشعار التعليق
                        // هنا بتحتاجي إيميل الأدمن
                        // بتجيبيه من الـ Tenant DB لاحقاً
                    }

                    _logger.LogWarning(
                        "Center {CenterId} suspended", expired.CenterId);
                }
            }

            await sharedDb.SaveChangesAsync();
        }

        private async Task SendRenewalReminders()
        {
            using var scope = _scopeFactory.CreateScope();
            var sharedDb = scope.ServiceProvider.GetRequiredService<SharedDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var reminderDate = today.AddDays(7);


            var expiringSubscriptions = await sharedDb.CenterSubscriptions
                .Include(s => s.Center)
                .Where(s => s.Status == SubscriptionStatus.Active
                         && s.EndDate == reminderDate)
                .ToListAsync();

            foreach (var subscription in expiringSubscriptions)
            {
                if (subscription.Center?.AdminWhatsappNumber != null)
                {
                    await emailService.SendSubscriptionRenewalReminderAsync(
                        subscription.Center.AdminWhatsappNumber,
                        subscription.Center.Name,
                        subscription.EndDate
                    );

                    _logger.LogInformation(
                        "Renewal reminder sent to center {CenterId}", subscription.CenterId);
                }
            }
        }
    }
}
