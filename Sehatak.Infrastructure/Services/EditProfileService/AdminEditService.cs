using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.EditProfile.EditProfileActors;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.IProfileInterface.ProfileAdmin;
using Sehatak.Domain.Entities.SharedEntities;
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
                if (request.LogoUrl == null)
                    throw new BusinessException("General.NotFound");

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

        public Task<EditSttafInformationResponse> EditSttafInformation(int adminId, EditSttafInformationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
