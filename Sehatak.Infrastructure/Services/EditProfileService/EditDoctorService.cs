using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.EditProfile.EditProfileActors;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.IProfileInterface.ProfileDoctor;
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
    public class EditDoctorService : IProfileDoctor
    {
        private readonly SharedDbContext sharedDbContext;
        private readonly TenantDbContextFactory contextFactory;
        public EditDoctorService(SharedDbContext sharedDbContext ,  TenantDbContextFactory contextFactory)
        {
            this.sharedDbContext = sharedDbContext;
            this.contextFactory = contextFactory;
        }

        public async Task<EditDoctorInformationResponse> EditDoctorInformation(int centerId, int userId, EditDoctorInformationRequest request)
        {
            var center = await sharedDbContext.MedicalCenters
                .FirstOrDefaultAsync(c => c.Id == centerId && c.CenterStatus == CenterStatus.Active);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            using var db = contextFactory.CreateForCenter(centerId);

            var doctor = await db.Doctors
                .Include(u => u.user)
                .FirstOrDefaultAsync(d => d.Id == userId && d.user.isActive);

            if (doctor == null)
                throw new BusinessException("Auth.Forbidden");

            if (request.firstNmae != null)
                doctor.user.firstName = request.firstNmae;

            if (request.lastNmae != null)
                doctor.user.lastName = request.lastNmae;

            if (request.address != null)
                doctor.user.address = request.address;

            if (request.city != null)
                doctor.user.city = request.city;

            if (request.phoneNumber != null)
                doctor.user.phoneNumber = request.phoneNumber;

            if (request.Specialization != null)
                doctor.Specialization = request.Specialization;

            if (request.Bio != null)
                doctor.Bio = request.Bio;



            if (request.profileImage != null)
            {
                if (!string.IsNullOrEmpty(doctor.user.ProfileImageUrl))
                    DeleteImageFile(doctor.user.ProfileImageUrl);

                var fileName = Guid.NewGuid() + Path.GetExtension(request.profileImage.FileName);
                var path = Path.Combine("wwwroot/uploads/receipts", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await request.profileImage.CopyToAsync(stream);
                }

                doctor.user.ProfileImageUrl = $"/uploads/receipts/{fileName}";
            }
            else if (request.RemoveProfileImage)
            {
                if (!string.IsNullOrEmpty(doctor.user.ProfileImageUrl))
                    DeleteImageFile(doctor.user.ProfileImageUrl);

                doctor.user.ProfileImageUrl = null;
            }


            await db.SaveChangesAsync();

            return new EditDoctorInformationResponse
            {
                DoctorId = userId,
                FullName = doctor.user.firstName + " " + doctor.user.lastName,
                PhoneNumber = doctor.user.phoneNumber,
                ProfileImageUrl = doctor.user.ProfileImageUrl,
                Address = doctor.user.address,
                City = doctor.user.city,
                Specialization = doctor.Specialization,
                Bio = doctor.Bio, 
                Message = "تم تحديث البيانات بنجاح"
            };
        }
        private void DeleteImageFile(string relativeUrl)
        {
            try
            {
                var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var fullPath = Path.Combine(webRoot, relativeUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }
            catch (Exception)
            {

            }
        }
    }
}
