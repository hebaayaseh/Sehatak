using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Sehatak.Infrastructure.CalculateSlot
{
    public class GenerateTheoreticalSlots
    {
        public List<TimeOnly?> GenerateTheoreticalSlot(TimeOnly startTime, TimeOnly endTime, int slotDurationMinutes)
        {
            var slots = new List<TimeOnly?>();
            var current = startTime;
            while (current.AddMinutes(slotDurationMinutes) <= endTime)
            {
                slots.Add(current);
                current = current.AddMinutes(slotDurationMinutes);
            }
            return slots;
        }
    }
}
