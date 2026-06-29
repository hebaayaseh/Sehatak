using BCrypt.Net;
using Castle.Core.Smtp;
using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs;
using Sehatak.Application.Interfaces;
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


namespace Sehatak.Infrastructure.Services;
    public class AuthService : IAuthService
    {
        private readonly TenantDbContextFactory tenantFactory;
        private readonly IEmailSenderService emailSender;
        private readonly JwtTokenGenerator jwtGenerator;

        public AuthService(TenantDbContextFactory tenantDbContextFactory, IEmailSenderService emailSenderService, JwtTokenGenerator jwtTokenGenerator)
        {
            this.tenantFactory = tenantDbContextFactory;
            this.emailSender = emailSenderService;
            this.jwtGenerator = jwtTokenGenerator;
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
        }

    
}

