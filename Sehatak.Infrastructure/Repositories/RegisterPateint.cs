using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sehatak.Infrastructure.Repositories
{
    public class RegisterPateint 
    {
        public readonly TenantDbContext tenantDbContext;
        public RegisterPateint(TenantDbContext tenantDbContext)
        {
            this.tenantDbContext = tenantDbContext;
        }
        public async void Register()
        {
            
        }
        
    }
}
