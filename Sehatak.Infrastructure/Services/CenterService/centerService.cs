using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.CreateCenterRequestDto;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.MedicalCenter;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.CenterService
{
    public class centerService : ICenterService
    {
        private readonly SharedDbContext sharedDbContext;
        private readonly TenantDbContextFactory tenantDbContextFactory;
        public centerService(SharedDbContext sharedDbContext , TenantDbContextFactory tenantDbContextFactory)
        {
            this.sharedDbContext = sharedDbContext;
            this.tenantDbContextFactory = tenantDbContextFactory;
        }

        public async Task<CenterResponseDto> CreateCenterAsync(createCenterRequestDto request)
        {
            var plan = await sharedDbContext.SubscriptionPlans.FindAsync(request.PlanId);
            if(plan==null) throw new BusinessException("Subscription.PlanNotFound");

            

            var center = new MedicalCenter { 
                Name=request.Name,
                Phone = request.Phone,
                Address = request.Address,
                RequiresPrepayment = request.RequiresPrepayment,
                PrepaymentAmount = request.PrepaymentAmount,
                RefundPolicyHours = request.RefundPolicyHours,
                PartialRefundPercent = request.PartialRefundPercent,
                CenterStatus = CenterStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            if (request.AdminWhatsappNumber != null)
            {
                center.AdminWhatsappNumber = request.AdminWhatsappNumber;
            }
            if(request.AddedBySuperAdminId != null)
            {
                center.AddedBySuperAdminId = request.AddedBySuperAdminId;
            }

            if (request.Logo != null)
            {

                var fileName = Guid.NewGuid() + Path.GetExtension(request.Logo.FileName);

                var path = Path.Combine("wwwroot/uploads/logos", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await request.Logo.CopyToAsync(stream);
                }
                center.LogoUrl = $"/uploads/logos/{fileName}";
            }
            // name of domain
            var centerUrl = $"{GenerateSlug(request.Name)}.sehatak.com";
            var urlExists = await sharedDbContext.MedicalCenters
              .AnyAsync(c => c.UniqueUrl == centerUrl);
            if (urlExists)
                throw new BusinessException("Center.UniqueUrlExists");

            center.UniqueUrl = centerUrl;

            await sharedDbContext.MedicalCenters.AddAsync(center);
            await sharedDbContext.SaveChangesAsync();

            var subscription = new CenterSubscription
            {
                CenterId = center.Id,
                PlanId = plan.Id,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(plan.DurationDays)),
                Status = SubscriptionStatus.Active,
                AmountPaid = plan.Price
            };

            await sharedDbContext.CenterSubscriptions.AddAsync(subscription);
            

            var planFeatures = await sharedDbContext.PlanFeatures.Where(p=>p.PlanId == request.PlanId).ToListAsync();
            foreach (var pf in planFeatures)
            {
                await sharedDbContext.CenterFeatures.AddAsync(new CenterFeature
                {
                    CenterId = center.Id,
                    FeatureId = pf.FeatureId,
                    IsEnabled = true
                });
            }

            

            await tenantDbContextFactory.CreateTenantDatabaseAsync(center.Id);

            center.CenterStatus = CenterStatus.Active;
            await sharedDbContext.SaveChangesAsync();

            var enabledFeatureNames = await sharedDbContext.CenterFeatures
                .Where(c=>c.CenterId == center.Id)
                .Include(c=>c.Feature)
                .Select(c=>c.Feature.NameOfFeature)
                .ToListAsync();
            
            return new CenterResponseDto {
                Id = center.Id,
                Name = center.Name,
                UniqueUrl = center.UniqueUrl,
                Status = center.CenterStatus.ToString(),
                EnabledFeatures = enabledFeatureNames
            };

            throw new BusinessException("Center.Activated");
            throw new BusinessException("Center.Created");

        }
        public string GenerateSlug(string name)
        {
            return name
                  .Trim()
                  .ToLower()
                  .Replace(" ", "-");
        }
    }
}
