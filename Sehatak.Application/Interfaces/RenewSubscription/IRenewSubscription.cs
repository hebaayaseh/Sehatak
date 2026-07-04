using Sehatak.Application.DTOs.RenewSubscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.RenewSubscription
{
    public interface IRenewSubscription
    {
        Task<bool> RenewActiveSubscriptionAsync(int centerId,RenewSubscriptionRequest request);
        Task<bool> RenewExpiredSubscriptionAsync(int centerId,RenewSubscriptionRequest request);
    }
}
