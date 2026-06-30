using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.AuthPatient
{
    public interface IEmailSenderService
    {
        Task SendOtpEmailAsync(string toEmail, string code);
    }
}
