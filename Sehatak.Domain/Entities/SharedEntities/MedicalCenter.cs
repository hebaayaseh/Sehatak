using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.SharedEnums;
using System.ComponentModel.DataAnnotations;

namespace Sehatak.Domain.Entities.SharedEntities
{
    public class MedicalCenter
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string UniqueUrl { get; set; } = string.Empty;

        public string Phone { get; set; }

        public string Address { get; set; }

        public string? LogoUrl { get; set; }

        public BookingType BookingType { get; set; } = BookingType.TimeSlot;

        public bool RequiresPrepayment { get; set; } = false;

        public decimal PrepaymentAmount { get; set; } = 0;

        public int RefundPolicyHours { get; set; } = 24;

        public decimal PartialRefundPercent { get; set; }

        public CenterStatus CenterStatus { get; set; } = CenterStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? AddedBySuperAdminId { get; set; }
        // The notifications sent contain subscription information : 
        public string? AdminWhatsappNumber { get; set; }

        // Navigation Properties :
        public ICollection<CenterSubscription> Subscriptions { get; set; } = new List<CenterSubscription>();
        public SuperAdmin AddedByAdmin { get; set; }
        public ICollection<CenterFeature> Features { get; set; } = new List<CenterFeature>();
    }
}
