
namespace Sehatak.Domain.Entities
{
    public class StaffShift
    {

        public int Id { get; set; }

        public int staffId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public string ShiftName { get; set; } = string.Empty;
        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public bool IsActive { get; set; } = true;

        //  Navigation Properties :
        public User User { get; set; } = null!;
    }
}
