using Sehatak.Domain.Entities.TenantEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Entities.SharedEntities
{
    public class emailVerificationCode
    {
        public int Id { get; set; }
        public int SuperAdminId { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Purpose { get; set; } = string.Empty;

        public string? PendingValue { get; set; }
        // Navigation property 
        public SuperAdmin superAdmin { get; set; } = null!;
    }
}
