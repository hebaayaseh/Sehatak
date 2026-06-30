using Microsoft.Extensions.Configuration;
using Sehatak.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.PatientRegisterAuth;
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _config;

        public EmailSenderService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendOtpEmailAsync(string toEmail, string code)
        {
            using var client = new SmtpClient(
                _config["Email:SmtpHost"],
                int.Parse(_config["Email:SmtpPort"]!))
            {
                Credentials = new NetworkCredential(
                    _config["Email:Username"],
                    _config["Email:Password"]),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(_config["Email:From"]!, _config["Email:DisplayName"]),
                Subject = "رمز تأكيد حسابك - Sehatak",
                Body = $"رمز التحقق الخاص بك هو: {code}\nصالح لمدة 10 دقائق.",
                IsBodyHtml = false
            };
            message.To.Add(toEmail);

            await client.SendMailAsync(message);
        }
    }

