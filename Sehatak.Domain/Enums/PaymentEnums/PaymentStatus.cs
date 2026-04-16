using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Domain.Enums.PaymentEnums
{
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        Refunded = 3,
        Failed = 4
    }

}
