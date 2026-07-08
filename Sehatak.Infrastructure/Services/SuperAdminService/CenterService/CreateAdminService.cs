using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.CreateCenterRequestDto;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.IEmail;
using Sehatak.Application.Interfaces.MedicalCenter;
using Sehatak.Domain.Entities.TenantEntities;
using Sehatak.Domain.Enums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdminService.CenterService
{
    public class CreateAdminService : ICreateAdminService
    {
        private readonly TenantDbContextFactory tenantDbContext;
        private readonly SharedDbContext sharedDbContext;
        private readonly IEmailService _emailService;
        public CreateAdminService(TenantDbContextFactory tenantDbContext, SharedDbContext sharedDbContext , IEmailService _emailService)
        {
            this.tenantDbContext = tenantDbContext;
            this.sharedDbContext = sharedDbContext;
            this._emailService = _emailService;
        }

        public async Task<CreateAdminResponseDto> CreateAdminAsync(int centerId, CreateAdminRequestDto request)
        {
            var center = await sharedDbContext.MedicalCenters
                .FindAsync(centerId);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            using var db = tenantDbContext.CreateForCenter(centerId);

            var emailExists = await db.Users.AnyAsync(u => u.email == request.Email);
            if (emailExists)
                throw new BusinessException("Auth.EmailExists");

            var tempPassword = GenerateTempPassword();

            var admin = new User
            {
                firstName = request.FirstName,
                lastName = request.LastName,
                address = request.Address,
                city = request.City,
                phoneNumber = request.PhoneNumber,
                email = request.Email,
                passwordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword),
                role = userRole.Admin,
                isActive = true,
                createdAt = DateTime.UtcNow
            };
            await db.Users.AddAsync(admin);
            await db.SaveChangesAsync();

            await _emailService.SendTempPasswordAsync(
                request.Email, name: $"{request.FirstName} {request.LastName}", tempPassword,center.Name
                );

            return new CreateAdminResponseDto
            {
                UserId = admin.Id,
                FullName = $"{admin.firstName} {admin.lastName}",
                Email = admin.email!,
                Message = "تم إنشاء حساب الأدمن وإرسال كلمة السر المؤقتة بالإيميل."
            };
        }
        private string GenerateTempPassword()
        {
            const string chars = "ABCDEFGHJKMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}
