using Sehatak.Domain.Enums;

namespace Sehatak.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string firstName {  get; set; }
        public string lastName { get; set; }
        public string? email { get; set; }
        public string passwordHash { get; set; } = string.Empty;
        public string? phoneNumber { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public userRole role { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public bool isActive { get; set; }
        public DateTime lastLogin { get; set; }
        public string? ProfileImageUrl { get; set; }

        // Navigation Properties :
        // use it because the Doctor and Pation have unique table 
        public Doctor? doctor { get; set; }

        public Patient? patient { get; set; }
        public ICollection<Notification> notification { get; set; } = new List<Notification>();
        public ICollection<Chat> sentMessages { get; set; } = new List<Chat>();
        public ICollection<Chat> receivedMessages {  get; set; } = new List<Chat>();
        public ICollection<StaffShift> Shifts { get; set; } = new List<StaffShift>();

    }
}
