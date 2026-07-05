using DocumentFormat.OpenXml.Spreadsheet;
using Sehatak.Application.DTOs.EditProfile;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.PatienRegisterDto;
using Sehatak.Application.DTOs.SuperAdminProfile;
using Sehatak.Application.Interfaces.AuthPatient;
using Sehatak.Application.Interfaces.ISuperDaminProfile;
using Sehatak.Domain.Entities;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Infrastructure.Data;
using Sehatak.Infrastructure.Security;
using Sehatak.Infrastructure.Services.PatientRegisterAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdminAuth
{
    public class SuperAdminProfileService : IProfile 
    {
        private readonly SharedDbContext sharedDbContext;
        public SuperAdminProfileService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public async Task<EmailResponse> EditEmail(int superAdminId, EditEmailRequest request)
        {
            var superAdmin = await sharedDbContext.SuperAdmins.FindAsync(superAdminId);
            if (superAdmin == null)
                throw new BusinessException("Auth.Unauthorized");

            if (superAdmin.Email == request.Email)
                throw new BusinessException("General.NotFound");

            var editEmail = new SuperAdmin 
            { 
                Email = request.Email,
            };

            await sharedDbContext.SaveChangesAsync();

            return new EmailResponse
            {
                Email = editEmail.Email,
            };

        }

        public async Task<NameResponse> EditName(int superAdminId, EditNameRequest request)
        {
            var superAdmin = await sharedDbContext.SuperAdmins.FindAsync(superAdminId);
            if (superAdmin == null)
                throw new BusinessException("Auth.Unauthorized");
            if (superAdmin.Name == request.Name)
                throw new BusinessException("General.NotFound");

            var name = new SuperAdmin
            {
                Name = request.Name
            };

            await sharedDbContext.SaveChangesAsync();
            return new NameResponse { Name = name.Name };


        }

        public async Task<PasswordResponse> EditPassword(int superAdminId, EditPasswordRequest request)
        {
            var superAdmin = await sharedDbContext.SuperAdmins.FindAsync(superAdminId);
            if (superAdmin == null)
                throw new BusinessException("Auth.Unauthorized");
            request.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
            request.ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(request.ConfirmPassword);

            if (request.PasswordHash != request.ConfirmPassword)
                throw new BusinessException("General.NotFound");

            if (superAdmin.PasswordHash == request.PasswordHash)
                throw new BusinessException("General.NotFound");

            var newPassword = new SuperAdmin
            {
                PasswordHash = request.PasswordHash
            };

            await sharedDbContext.SaveChangesAsync();
            return new PasswordResponse { message = "Password Update Succses" };
        }

        public async Task<ProfileImageResponse> EditProfileImage(int superAdminId, EditProfileImageRequest request)
        {
            var superAdmin = await sharedDbContext.SuperAdmins.FindAsync(superAdminId);
            if (superAdmin == null)
                throw new BusinessException("Auth.Unauthorized");
            if (request.ImageFile == null)
                throw new BusinessException("General.NotFound");

            var fileName = Guid.NewGuid() + Path.GetExtension(request.ImageFile.FileName);

            var path = Path.Combine("wwwroot/uploads/logos", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await request.ImageFile.CopyToAsync(stream);
            }

            superAdmin.ProfileImageUrl = $"/uploads/logos/{fileName}";

            var newImage = new SuperAdmin
            {
                ProfileImageUrl = superAdmin.ProfileImageUrl
            };
            await sharedDbContext.SaveChangesAsync();

            return new ProfileImageResponse
            {
                profileImage = newImage.ProfileImageUrl
            };

        }

        public async Task<ProfileResponse> ViewProfile(int superAdminId)
        {
            var superAdmin = await sharedDbContext.SuperAdmins.FindAsync(superAdminId);
            if (superAdmin == null)
                throw new BusinessException("Auth.Unauthorized");
            string? Image = null;
            if (!string.IsNullOrEmpty(superAdmin.ProfileImageUrl))
            {
                Image = superAdmin.ProfileImageUrl;
            }

            return new ProfileResponse
            {
                Name = superAdmin.Name,
                Email = superAdmin.Email,
                Phone = superAdmin.phone,
                ProfileImage = Image
            };

        }
    }
}
