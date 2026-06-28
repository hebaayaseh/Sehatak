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
    public class UserRepository
    {
        public readonly TenantDbContext tenantDbContext;
        public UserRepository(TenantDbContext tenantDbContext)
        {
            this.tenantDbContext = tenantDbContext;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await tenantDbContext.Users.FirstOrDefaultAsync(u => u.email == email);
        }
        public async Task addUser(User user)
        {
            await tenantDbContext.Users.AddAsync(user);
            
        }
        public async Task saveChange()
        {
            await tenantDbContext.SaveChangesAsync();
        }

    }
}
