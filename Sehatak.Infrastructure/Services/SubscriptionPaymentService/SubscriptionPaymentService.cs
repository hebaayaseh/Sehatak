using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Hosting;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.RecordPaymentRequestDto;
using Sehatak.Application.Interfaces.ISubscriptionPaymentService;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SubscriptionPaymentService
{
    public class SubscriptionPaymentService : ISubscriptionPayment
    {
        private readonly SharedDbContext sharedDbContext;
        private readonly IHostEnvironment _env;
        
        public SubscriptionPaymentService(SharedDbContext sharedDb, IHostEnvironment env)
        {
            sharedDbContext = sharedDb;
            _env = env;
        }
        public async Task<bool> ConfirmPaymentAsync(int paymentId, int superAdminId)
        {
            var payment = await sharedDbContext.subscriptionPayments
                .FirstOrDefaultAsync(p=>p.Id== paymentId);

            if (payment == null)
                throw new BusinessException("Payment.NotFound");
            if(payment.RecordedBySuperAdminId!=null)
                throw new BusinessException("Payment.Success");

            var subscription = payment.Subscription;
            
            var center = await sharedDbContext.MedicalCenters.FindAsync(subscription.CenterId);
            if (center == null)
                throw new BusinessException("Center.NotFound");

            payment.RecordedBySuperAdminId = superAdminId;

            subscription.Status = SubscriptionStatus.Active;

            var oldFeature = await sharedDbContext.CenterFeatures
                .Where(c => c.CenterId == center.Id)
                .ToListAsync();
            sharedDbContext.CenterFeatures.RemoveRange(oldFeature);

            var newFeatureIds = await sharedDbContext.PlanFeatures
                .Where(c => c.PlanId == subscription.PlanId)
                .Select(f => f.FeatureId)
                .ToListAsync();

            foreach(var feature in newFeatureIds)
            {
                sharedDbContext.CenterFeatures.Add(new CenterFeature
                {
                    CenterId = center.Id,
                    FeatureId = feature,
                    IsEnabled = true
                });
            }

            if(center!=null&&center.CenterStatus==CenterStatus.Suspended)
                center.CenterStatus = CenterStatus.Active;

            await sharedDbContext.SaveChangesAsync();
            return true;

        }

        public async Task<List<PaymentResponseDto>> GetCenterPaymentsAsync(int centerId)
        {
            var center = await sharedDbContext.MedicalCenters.FirstOrDefaultAsync(c => c.Id == centerId);
            if (center == null)
                throw new BusinessException("Center.NotFound");

            return await sharedDbContext.subscriptionPayments
                .Include(c => c.Center)
                .Include(s => s.RecordedBy)
                .Where(c => c.CenterId == centerId)
                .OrderByDescending(p => p.PaidAt)
                .Select(p => new PaymentResponseDto
                {
                    Id = p.Id,
                    CenterId = p.CenterId,
                    CenterName = p.Center.Name,
                    SubscriptionId = p.SubscriptionId,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    ReferenceNumber = p.ReferenceNumber,
                    ReceiptImageUrl = p.ReceiptImageUrl,
                    PaidAt = p.PaidAt,
                    RecordedBy = p.RecordedBy != null ? p.RecordedBy.Name : "Pending",
                    Notes = p.Notes,
                    IsConfirmed = p.RecordedBySuperAdminId != null

                }).ToListAsync();

        }

        


        }
    }
}
