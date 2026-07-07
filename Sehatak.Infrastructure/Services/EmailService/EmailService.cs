// Sehatak.Infrastructure/Services/EmailService.cs
using Microsoft.Extensions.Configuration;
using Sehatak.Application.Interfaces;
using Sehatak.Application.Interfaces.IEmail;
using System.Net;
using System.Net.Mail;

namespace Sehatak.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        private async Task SendAsync(string toEmail, string subject, string body)
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
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(toEmail);

            await client.SendMailAsync(message);
        }

        public async Task SendOtpAsync(string toEmail, string code, string purpose)
        {
            var subject = purpose switch
            {
                "register" => "تأكيد حسابك - Sehatak",
                "change-email" => "تأكيد تغيير الإيميل - Sehatak",
                "change-password" => "تأكيد تغيير كلمة السر - Sehatak",
                _ => "رمز التحقق - Sehatak"
            };

            var body = $@"
                <div dir='rtl' style='font-family:Arial;padding:20px'>
                    <h2>رمز التحقق الخاص بك</h2>
                    <p style='font-size:32px;font-weight:bold;letter-spacing:8px'>{code}</p>
                </div>";

            await SendAsync(toEmail, subject, body);
        }

        public async Task SendTempPasswordAsync(string toEmail, string name, string tempPassword, string centerName)
        {
            var body = $@"
                <div dir='rtl' style='font-family:Arial;padding:20px'>
                    <h2>مرحباً {name}</h2>
                    <p>تم إنشاء حسابك بمركز <strong>{centerName}</strong> على منصة Sehatak.</p>
                    <p>كلمة السر المؤقتة الخاصة بك:</p>
                    <p style='font-size:24px;font-weight:bold;background:#f0f0f0;padding:10px;border-radius:8px'>{tempPassword}</p>
                    <p style='color:red'>يرجى تغيير كلمة السر فور تسجيل الدخول.</p>
                </div>";

            await SendAsync(toEmail, "بيانات حسابك - Sehatak", body);
        }

        public async Task SendSubscriptionRenewalReminderAsync(string toEmail, string centerName, DateOnly endDate)
        {
            var body = $@"
                <div dir='rtl' style='font-family:Arial;padding:20px'>
                    <h2>تنبيه تجديد اشتراك</h2>
                    <p>اشتراك مركز <strong>{centerName}</strong> سينتهي بتاريخ <strong>{endDate}</strong>.</p>
                    <p>يرجى التواصل مع الإدارة لتجديد الاشتراك.</p>
                </div>";

            await SendAsync(toEmail, $"تنبيه: اشتراك {centerName} سينتهي قريباً", body);
        }

        public async Task SendPaymentConfirmedAsync(string toEmail, string centerName, decimal amount)
        {
            var body = $@"
                <div dir='rtl' style='font-family:Arial;padding:20px'>
                    <h2>تأكيد استلام الدفع</h2>
                    <p>تم استلام دفعة اشتراك مركز <strong>{centerName}</strong>.</p>
                    <p>المبلغ: <strong>{amount} ₪</strong></p>
                    <p>تم تفعيل اشتراكك بنجاح.</p>
                </div>";

            await SendAsync(toEmail, $"تأكيد دفع - {centerName}", body);
        }
    }
}
