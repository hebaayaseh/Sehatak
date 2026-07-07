using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sehatak.Application.DTOs.Email;
using Sehatak.Application.Interfaces.IEmail;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using Sehatak.Infrastructure.Services.SuperAdminService.Background;

namespace Sehatak.API.Controllers.SuperAdminController.Emails
{
    [ApiController]
    [Route("api/send_emails")]
    public class SendEmailsController : ControllerBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public SendEmailsController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpPost("send-email")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailDto request)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var sharedDb = scope.ServiceProvider
                .GetRequiredService<SharedDbContext>();
            var emailService = scope.ServiceProvider
                .GetRequiredService<IEmailService>();

            switch (request.Target)
            {

                case "specific":
                    var center = await sharedDb.MedicalCenters
                        .FindAsync(request.CenterId);
                    if (center?.AdminEmail == null)
                        return BadRequest("Center email not found");

                    await emailService.SendCustomMessageAsync(center.AdminEmail, request.Subject, request.Message);
                    break;


                case "active":
                    var activeEmails = await sharedDb.MedicalCenters
                        .Where(c => c.CenterStatus == CenterStatus.Active
                                 && c.AdminEmail != null)
                        .Select(c => c.AdminEmail!)
                        .ToListAsync();

                    await emailService.SendBulkAsync(
                        activeEmails, request.Subject, request.Message);
                    break;


                case "expired":
                    var expiredCenterIds = await sharedDb.CenterSubscriptions
                        .Where(s => s.Status == SubscriptionStatus.Expired)
                        .Select(s => s.CenterId)
                        .Distinct()
                        .ToListAsync();

                    var expiredEmails = await sharedDb.MedicalCenters
                        .Where(c => expiredCenterIds.Contains(c.Id)
                                 && c.AdminEmail != null)
                        .Select(c => c.AdminEmail!)
                        .ToListAsync();

                    await emailService.SendBulkAsync(
                        expiredEmails, request.Subject, request.Message);
                    break;

                default:
                    return BadRequest("Invalid target");
            }

            return Ok(new { message = "Emails sent successfully" });
        }
    }
}
