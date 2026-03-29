using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Enums
{
    public enum NotificationType
    {
        Appointment = 0,
        Cancellation = 1,
        Reminder = 2,
        LabResult = 3,
        System = 4,
        Waitlist = 5
    }
}
