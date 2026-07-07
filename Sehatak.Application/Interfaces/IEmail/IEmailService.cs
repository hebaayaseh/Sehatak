using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.IEmail
{
    public interface IEmailService
    {
        Task SendOtpAsync(string toEmail, string code, string purpose);


        Task SendTempPasswordAsync(string toEmail, string name, string tempPassword, string centerName);


        Task SendSubscriptionRenewalReminderAsync(string toEmail, string centerName, DateOnly endDate);


        Task SendPaymentConfirmedAsync(string toEmail, string centerName, decimal amount);
    }
}
