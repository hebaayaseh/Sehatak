using Sehatak.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Entities.SharedEntities
{
    public class CenterRegistrationRequest
    {
        public int Id { get; set; }
        public string CenterName { get; set; } = string.Empty;
        public string CenterAddress { get; set; } = string.Empty;
        public string CenterPhone { get; set; } = string.Empty;

        public string AdminFullName { get; set; } = string.Empty;
        public string AdminEmail { get; set; } = string.Empty;
        public string AdminPhone { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public CenterRegistrationStatus Status { get; set; } = CenterRegistrationStatus.Pending;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }
        public string? RejectionReason { get; set; }


        public int? ReviewedBySuperAdminId { get; set; }
        public SuperAdmin? ReviewedBySuperAdmin { get; set; }

        public int? CreatedCenterId { get; set; }

        // Navigation Properties : 
        public MedicalCenter? CreatedCenter { get; set; }

    }
}
