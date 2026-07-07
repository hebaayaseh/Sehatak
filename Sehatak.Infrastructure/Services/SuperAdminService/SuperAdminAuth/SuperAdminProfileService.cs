using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdminService.SuperAdminAuth
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

            var exists = await sharedDbContext.SuperAdmins
                .AnyAsync(x => x.Email == request.Email);

            if (exists)
                throw new BusinessException("Auth.EmailExists");

            superAdmin.Email = request.Email;

            await sharedDbContext.SaveChangesAsync();

            return new EmailResponse
            {
                Email = superAdmin.Email,
            };

        }

        public async Task<NameResponse> EditName(int superAdminId, EditNameRequest request)
        {
            var superAdmin = await sharedDbContext.SuperAdmins.FindAsync(superAdminId);
            if (superAdmin == null)
                throw new BusinessException("Auth.Unauthorized");

            if (superAdmin.Name == request.Name)
                throw new BusinessException("General.NotFound");

            superAdmin.Name = request.Name;

            await sharedDbContext.SaveChangesAsync();

            return new NameResponse { Name = superAdmin.Name };


        }

        public async Task<PasswordResponse> EditPassword(int superAdminId, EditPasswordRequest request)
        {
            var superAdmin = await sharedDbContext.SuperAdmins.FindAsync(superAdminId);
            if (superAdmin == null)
                throw new BusinessException("Auth.Unauthorized");


            if (request.PasswordHash != request.ConfirmPassword)
                throw new BusinessException("General.NotFound");

            var isSamePassword = BCrypt.Net.BCrypt.Verify(request.PasswordHash, superAdmin.PasswordHash);
            if (isSamePassword)
                throw new BusinessException("General.NotFound");

            superAdmin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

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


            
            await sharedDbContext.SaveChangesAsync();

            return new ProfileImageResponse
            {
                profileImage = superAdmin.ProfileImageUrl
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
