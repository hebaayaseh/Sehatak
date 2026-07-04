using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.Background
{
    public class SubscriptionActivationService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SubscriptionActivationService> _logger;

        public SubscriptionActivationService(
            IServiceScopeFactory scopeFactory,
            ILogger<SubscriptionActivationService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAndActivateSubscriptions();
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // بيشتغل كل 24 ساعة
            }
        }

        private async Task CheckAndActivateSubscriptions()
        {
            using var scope = _scopeFactory.CreateScope();
            var sharedDb = scope.ServiceProvider.GetRequiredService<SharedDbContext>();

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            // 1. جيبي كل الاشتراكات النشطة المنتهية
            var expiredSubscriptions = await sharedDb.CenterSubscriptions
                .Where(s => s.Status == SubscriptionStatus.Active && s.EndDate <= today)
                .ToListAsync();

            foreach (var expired in expiredSubscriptions)
            {
                // 2. غيّري القديم لـ Expired
                expired.Status = SubscriptionStatus.Expired;

                // 3. شوفي إذا في Pending للمركز هاد
                var pending = await sharedDb.CenterSubscriptions
                    .FirstOrDefaultAsync(s => s.CenterId == expired.CenterId
                                           && s.Status == SubscriptionStatus.Pending);

                if (pending != null)
                {
                    // 4. فعّلي الاشتراك الجديد
                    pending.Status = SubscriptionStatus.Active;

                    // 5. احذفي features القديمة
                    var oldFeatures = await sharedDb.CenterFeatures
                        .Where(cf => cf.CenterId == expired.CenterId)
                        .ToListAsync();
                    sharedDb.CenterFeatures.RemoveRange(oldFeatures);

                    // 6. انسخي features الخطة الجديدة
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

                    _logger.LogInformation(
                        "Subscription activated for center {CenterId} with plan {PlanId}",
                        expired.CenterId, pending.PlanId);
                }
                else
                {
                    // ما في Pending — علّقي المركز
                    var center = await sharedDb.MedicalCenters.FindAsync(expired.CenterId);
                    if (center != null)
                        center.CenterStatus = CenterStatus.Suspended;

                    _logger.LogWarning(
                        "Center {CenterId} suspended — no pending subscription found",
                        expired.CenterId);
                }
            }

            await sharedDb.SaveChangesAsync();
        }
    }
}

