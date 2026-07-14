using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.EditProfile.EditProfileActors;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.IProfileInterface.ProfileAdmin;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.EditProfileService
{
    public class AdminEditService : IprofileAdmin
    {
        private readonly SharedDbContext sharedDbContext;
        private TenantDbContextFactory contextFactory; 
        public AdminEditService(SharedDbContext sharedDbContext ,  TenantDbContextFactory contextFactory)
        {
            this.sharedDbContext = sharedDbContext;
            this.contextFactory = contextFactory;
        }

        public async Task<EditCenterInformationResponse> EditCenterInformation(int centerId, EditCenterInformationRequest request)
        {
            var center = await sharedDbContext.MedicalCenters
                .FirstOrDefaultAsync(c => c.Id == centerId && c.CenterStatus == CenterStatus.Active);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            if (request.PartialRefundPercent != null)
                center.PartialRefundPercent = (decimal)request.PartialRefundPercent;

            if (request.PrepaymentAmount != null)
                center.PrepaymentAmount = (decimal)request.PrepaymentAmount;

            if (request.RequiresPrepayment != null)
                center.RequiresPrepayment = (bool)request.RequiresPrepayment;

            if (request.RefundPolicyHours != null)
                center.RefundPolicyHours = (int)request.RefundPolicyHours;

            if (request.AdminWhatsappNumber != null)
                center.AdminWhatsappNumber = request.AdminWhatsappNumber;

            if(request.Phone!=null)
                center.Phone = request.Phone;

            if (request.Address != null)
                center.Address = request.Address;

            if(request.LogoUrl!=null)
            {
                
                var fileName = Guid.NewGuid() + Path.GetExtension(request.LogoUrl.FileName);

                var path = Path.Combine("wwwroot/uploads/receipts", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await request.LogoUrl.CopyToAsync(stream);
                }

                center.LogoUrl = $"/uploads/receipts/{fileName}";

            }
            await sharedDbContext.SaveChangesAsync();

            return new EditCenterInformationResponse { 
            
                centerId=centerId,
                PartialRefundPercent = center.PartialRefundPercent,
                PrepaymentAmount = center.PrepaymentAmount,
                RefundPolicyHours = center.RefundPolicyHours,
                RequiresPrepayment = center.RequiresPrepayment,
                Address = center.Address,
                LogoUrl = center.LogoUrl,
                AdminWhatsappNumber = center.AdminWhatsappNumber,
                Phone = center.Phone

            };

        }

        public async Task<EditSttafInformationResponse> EditSttafInformation(int centerId , int adminId, EditSttafInformationRequest request)
        {
            var center = await sharedDbContext.MedicalCenters
                .FirstOrDefaultAsync(c => c.Id == centerId && c.CenterStatus == CenterStatus.Active);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            using var db = contextFactory.CreateForCenter(centerId);

            var admin = await db.Users
                .FirstOrDefaultAsync(a => a.Id == adminId && a.role == userRole.Admin && a.isActive);

            if (admin == null)
                throw new BusinessException("Auth.Forbidden");

            if (request.firstNmae != null)
                admin.firstName = request.firstNmae;

            if (request.lastNmae != null)
                admin.lastName = request.lastNmae;

            if (request.address != null)
                admin.address = request.address;

            if (request.city != null)
                admin.city = request.city;

            if(request.phoneNumber!=null)
                admin.phoneNumber=request.phoneNumber;

            if (request.profileImage != null)  
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(request.profileImage.FileName);
                var path = Path.Combine("wwwroot/uploads/receipts", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await request.profileImage.CopyToAsync(stream);
                }

                admin.ProfileImageUrl = $"/uploads/receipts/{fileName}";
            }

            await db.SaveChangesAsync();

            return new EditSttafInformationResponse
            {
                StaffId = adminId,
                FullName = admin.firstName + " " + admin.lastName,
                PhoneNumber = admin.phoneNumber,
                ProfileImageUrl = admin.ProfileImageUrl,
                Address = admin.address,
                City = admin.city,
                Message = "تم تحديث البيانات بنجاح"
            };

        }
    }
}
