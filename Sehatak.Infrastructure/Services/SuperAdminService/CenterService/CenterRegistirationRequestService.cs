using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.CreateCenterRequestDto;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.CenterRegistrationRequest;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdminService.CenterService
{
    public class CenterRegistirationRequestService : ICenterRegistration
    {
        private readonly SharedDbContext sharedDbContext;
        private TenantDbContextFactory contextFactory;
        public CenterRegistirationRequestService(SharedDbContext sharedDbContext , TenantDbContextFactory contextFactory)
        {
            this.sharedDbContext = sharedDbContext;
            this.contextFactory=contextFactory;
        }

        public async Task<CenterRegistrationResponseDto> CenterRequestAsync(CenterRegistrationRequestDto request)
        {
            var center = await sharedDbContext.MedicalCenters
                .FirstOrDefaultAsync(c => c.Name == request.CenterName);

            if (center != null)
                throw new BusinessException("Center.UrlExists");

            var pendingRequestExists = await sharedDbContext.centerRegistrationRequests
                .AnyAsync(r => r.AdminEmail == request.AdminEmail
                            && r.Status == CenterRegistrationStatus.Pending);
            if (pendingRequestExists)
                throw new BusinessException("CenterRegistration.AlreadyPending");


            string? receiptImageUrl = null;
            if (request.LogoUrl != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
                var extension = Path.GetExtension(request.LogoUrl.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                    throw new BusinessException("Validation.InvalidFileType");

                if (request.LogoUrl.Length > 5 * 1024 * 1024)
                    throw new BusinessException("Validation.FileTooLarge");

                var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var uploadsFolder = Path.Combine(webRoot, "uploads", "receipts");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.LogoUrl.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await request.LogoUrl.CopyToAsync(stream);

                receiptImageUrl = $"/uploads/receipts/{fileName}";

            }
            
            var creatCenter = new CenterRegistrationRequest
            {
                AdminEmail = request.AdminEmail,
                AdminPhone = request.AdminPhone,
                CenterAddress = request.CenterAddress,
                CenterName = request.CenterName,
                CenterPhone = request.CenterPhone,
                AdminFirstName = request.AdminFirstName,
                AdminLastName = request.AdminLastName,
                PlanId = request.PlanId,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash),
                RequestedAt = request.RequestedAt,
               
            };
            await sharedDbContext.centerRegistrationRequests.AddAsync(creatCenter);
            await sharedDbContext.SaveChangesAsync();

            return new CenterRegistrationResponseDto
            {
                Id = creatCenter.Id,
                CenterName = creatCenter.CenterName,
                CenterPhone = creatCenter.CenterPhone,
                CenterAddress = creatCenter.CenterAddress,
                AdminEmail = creatCenter.AdminEmail,
                AdminFirstName = creatCenter.AdminFirstName,
                AdminLastName = creatCenter.AdminLastName,
                PlanId = creatCenter.PlanId,
                AdminPhone = creatCenter.AdminPhone,
                Status = CenterRegistrationStatus.Pending,
                RequestedAt = creatCenter.RequestedAt,
                ReviewedAt = null,
                RejectionReason = null,
                ReviewedBySuperAdminId = null,
            };
        }

        public async Task<List<CenterRegistrationResponseDto>> GetCentersRegisteration()
        {
            return await sharedDbContext.centerRegistrationRequests
                .Where(c => c.ReviewedBySuperAdminId == null && c.Status==CenterRegistrationStatus.Pending)
                .OrderByDescending(r => r.RequestedAt)
                .Select(c=> new CenterRegistrationResponseDto
                {
                    AdminEmail = c.AdminEmail,
                    AdminFullName = c.AdminFullName,
                    AdminPhone = c.AdminPhone,
                    CenterAddress = c.CenterAddress,
                    CenterName= c.CenterName,
                    CenterPhone= c.CenterPhone,
                    CreatedCenterId=c.CreatedCenterId,
                    RequestedAt=c.RequestedAt,
                    ReviewedBySuperAdmin=c.ReviewedBySuperAdmin,
                    ReviewedBySuperAdminId=c.ReviewedBySuperAdminId,
                    Status = c.Status,
                    RejectionReason = c.RejectionReason

                }).ToListAsync();
        }
        public async Task<CenterRegistrationResponseDto?> GetCenterRegistrationAsync(int centerId)
        {
            var center = await sharedDbContext.centerRegistrationRequests
                .FindAsync(centerId);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            return await sharedDbContext.centerRegistrationRequests
                .Where(c => c.ReviewedBySuperAdminId == null && c.Id == centerId)
                .Select(c => new CenterRegistrationResponseDto
                {
                    AdminEmail = c.AdminEmail,
                    AdminFirstName = c.AdminFirstName,
                    AdminLastName = c.AdminLastName,
                    PlanId = c.PlanId,
                    AdminPhone = c.AdminPhone,
                    CenterAddress = c.CenterAddress,
                    CenterName = c.CenterName,
                    CenterPhone = c.CenterPhone,
                    CreatedCenterId = c.CreatedCenterId,
                    RequestedAt = c.RequestedAt,
                    ReviewedBySuperAdmin = c.ReviewedBySuperAdmin,
                    ReviewedBySuperAdminId = c.ReviewedBySuperAdminId,
                    Status = c.Status,
                    RejectionReason = c.RejectionReason

                }).FirstOrDefaultAsync();
        }

        public async Task<bool> ApproveCenterRequest(int centerId,int superAdminId)
        {
            var superAdmin = await sharedDbContext.SuperAdmins
                .FindAsync(superAdminId);

            if (superAdmin == null)
                throw new BusinessException("Auth.Unauthorized");

            var request = await sharedDbContext.centerRegistrationRequests
                .FirstOrDefaultAsync(r => r.Id == centerId);

            if (request == null)
                throw new BusinessException("CenterRegistration.NotFound");

            if (request == null)
                throw new BusinessException("CenterRegistration.NotFound");

            if (request.Status != CenterRegistrationStatus.Pending)
                throw new BusinessException("CenterRegistration.AlreadyReviewed");

            var newCenter = new MedicalCenter
            {
                Name = request.CenterName,
                AdminEmail = request.AdminEmail,
                Phone = request.CenterPhone,
                AdminWhatsappNumber = request.AdminPhone,
                CreatedAt = request.RequestedAt,
                CenterStatus = CenterStatus.Active,
                PartialRefundPercent = request.PartialRefundPercent,
                RefundPolicyHours = request.RefundPolicyHours,
                RequiresPrepayment = request.RequiresPrepayment,
                PrepaymentAmount = request.PrepaymentAmount,
                LogoUrl = request.logo
            };
            await sharedDbContext.MedicalCenters.AddAsync(newCenter);
            await sharedDbContext.SaveChangesAsync();

            request.Status = CenterRegistrationStatus.Approved;
            request.ReviewedAt = DateTime.UtcNow;
            request.ReviewedBySuperAdminId = superAdminId;
            request.CreatedCenterId = newCenter.Id;
            await sharedDbContext.SaveChangesAsync();

            return true;

        }

    }
}
