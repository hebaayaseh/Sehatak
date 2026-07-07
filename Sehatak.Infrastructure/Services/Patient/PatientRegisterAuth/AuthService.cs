using BCrypt.Net;
using Castle.Core.Smtp;
using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.PatienRegisterDto;
using Sehatak.Application.Interfaces.AuthPatient;
using Sehatak.Application.Interfaces.IEmail;
using Sehatak.Domain.Entities;
using Sehatak.Domain.Enums;
using Sehatak.Infrastructure.Data;
using Sehatak.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;


namespace Sehatak.Infrastructure.Services.Patient.PatientRegisterAuth;
    public class AuthService : IAuthService
    {
        private readonly TenantDbContextFactory tenantFactory;
        private readonly IEmailService emailSender;
        private readonly JwtTokenGenerator jwtGenerator;

        public AuthService(TenantDbContextFactory tenantDbContextFactory, IEmailService emailSenderService, JwtTokenGenerator jwtTokenGenerator)
        {
            tenantFactory = tenantDbContextFactory;
            emailSender = emailSenderService;
            jwtGenerator = jwtTokenGenerator;
        }

        public async Task<RegisterRequestDto> RegisterAsync(RegisterRequestDto request)
        {
            var db = tenantFactory.CreateForCenter(request.CenterId);
            var existing = await db.Users.FirstOrDefaultAsync(u => u.email == request.email);
            if (existing != null)
                throw new BusinessException("Auth.EmailExists");

            var user = new User
            {
                firstName = request.firstName,
                lastName = request.lastName,
                email = request.email,
                passwordHash = BCrypt.Net.BCrypt.HashPassword(request.password),
                phoneNumber = request.phoneNumber,
                address = request.address,
                city = request.city,
                role = userRole.Patient,
                isActive = false,
                createdAt = DateTime.UtcNow
            };
        if(request.ProfileImage != null)
        {

            var fileName = Guid.NewGuid() + Path.GetExtension(request.ProfileImage.FileName);

            var path = Path.Combine("wwwroot/uploads/logos", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await request.ProfileImage.CopyToAsync(stream);
            }

            user.ProfileImageUrl = $"/uploads/logos/{fileName}";
        }
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var code = new Random().Next(100000, 999999).ToString();
            db.EmailVerificationCodes.Add(new EmailVerificationCode
            {
                UserId = user.Id,
                Code = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)
            });
            await db.SaveChangesAsync();

            await emailSender.SendOtpEmailAsync(user.email!, code);
        

        return request;


        }

        public async Task<VerifyOtpResponseDto?> VerifyOtpAsync(VerifyOtpRequestDto request)
        {
            using var db = tenantFactory.CreateForCenter(request.CenterId);

            var user = await db.Users.FirstOrDefaultAsync(u => u.email == request.Email);
            if (user == null) return null;

            var validCode = await db.EmailVerificationCodes
                .Where(c => c.UserId == user.Id
                         && c.Code == request.code
                         && !c.IsUsed
                         && c.ExpiresAt > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if (validCode == null) return null;

            user.isActive = true;
            validCode.IsUsed = true;
            await db.SaveChangesAsync();

            var token = jwtGenerator.GenerateToken(
                userId: user.Id,
                name: $"{user.firstName} {user.lastName}",
                email: user.email!,
                role: user.role.ToString(),
                centerId: request.CenterId
            );

            return new VerifyOtpResponseDto { Token = token };
        throw new BusinessException("GeneralSuccess");
        }

    
}

