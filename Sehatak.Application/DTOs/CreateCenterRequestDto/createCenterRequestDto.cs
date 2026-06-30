using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.SharedEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.CreateCenterRequestDto
{
    public class createCenterRequestDto
    {
        [Required, MaxLength(256)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string UniqueUrl { get; set; } = string.Empty;
        [Required, Phone, MaxLength(20)]
        public string Phone { get; set; }
        [Required, MaxLength(500)]
        public string Address { get; set; }

        public string? LogoUrl { get; set; }

        public BookingType BookingType { get; set; } 

        public bool RequiresPrepayment { get; set; } = false;

        public decimal PrepaymentAmount { get; set; } = 0;

        public int RefundPolicyHours { get; set; } = 24;

        public decimal PartialRefundPercent { get; set; }

        public CenterStatus CenterStatus { get; set; } = CenterStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public int PlanId { get; set; }
        public int? AddedBySuperAdminId { get; set; }
        
        public string? AdminWhatsappNumber { get; set; }
    }
}
