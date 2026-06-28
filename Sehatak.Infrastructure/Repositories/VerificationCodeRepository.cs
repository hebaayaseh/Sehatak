using Microsoft.EntityFrameworkCore;
using Sehatak.Domain.Entities;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Repositories
{
    public class VerificationCodeRepository
    {
        public readonly TenantDbContext tenantDbContext;
        public readonly UserRepository userRepository;
        public VerificationCodeRepository(TenantDbContext tenantDbContext, UserRepository userRepository)
        {
            this.tenantDbContext = tenantDbContext;
            this.userRepository = userRepository;
        }
        public async Task addAsync(EmailVerificationCode emailVerificationCode)
        {
            await tenantDbContext.EmailVerificationCodes.AddAsync(emailVerificationCode);
           
        }

        public async Task<EmailVerificationCode?> GetVerificationCodeAsync(int userId, string code)
        {
            return await tenantDbContext.EmailVerificationCodes
                .Where(u => u.UserId == userId
                && u.Code == code
                && u.ExpiresAt > DateTime.UtcNow
                && !u.IsUsed
                )
                .FirstOrDefaultAsync();
        }
        public async Task saveChange()
        {
            await tenantDbContext.SaveChangesAsync();
        }

    }
}
