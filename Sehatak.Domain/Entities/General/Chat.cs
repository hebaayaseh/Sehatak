using Sehatak.Domain.Entities.TenantEntities;
using System.ComponentModel.DataAnnotations;

namespace Sehatak.Domain.Entities.General
{
   public class Chat
    {
        [Key]
        public int Id { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }
        [Required]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        //  Navigation Properties :
        public User Sender { get; set; } = null!;
        public User Receiver { get; set; } = null!;
    }
}
