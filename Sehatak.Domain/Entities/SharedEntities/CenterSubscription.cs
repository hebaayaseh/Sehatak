using Sehatak.Domain.Enums.SharedEnums;
using System.ComponentModel.DataAnnotations;

namespace Sehatak.Domain.Entities.SharedEntities
{
    public class CenterSubscription
    {
        
        public int Id { get; set; }

        public int CenterId { get; set; }

        public int PlanId { get; set; }
        
        public DateOnly StartDate { get; set; }
        
        public DateOnly EndDate { get; set; }

        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

        public decimal AmountPaid { get; set; }
        // Special for the super admin
        public string? PaymentReference { get; set; }

        // Navigation Properties 
        public MedicalCenter Center { get; set; } = null!;
        public SubscriptionPlan Plan { get; set; } = null!;
    }
}
