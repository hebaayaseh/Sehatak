using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.FinancialReport;
using Sehatak.Application.Interfaces.IFinancialReports;
using Sehatak.Domain.Enums;
using Sehatak.Domain.Enums.PaymentEnums;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.FinancialReportServices
{
    public class FinancialReportAdminService : IFinancialReportAdmin
    {
        private SharedDbContext sharedDbContext;
        private TenantDbContextFactory contextFactory;
        public FinancialReportAdminService(SharedDbContext sharedDbContext, TenantDbContextFactory contextFactory)
        {
            this.sharedDbContext = sharedDbContext;
            this.contextFactory = contextFactory;
        }

        public async Task<byte[]> GenerateReportAsync(int centerId, int userId, FinancialReportRequestDto request)
        {
            var center = await sharedDbContext.MedicalCenters
                .FirstOrDefaultAsync(c => c.Id == centerId && c.CenterStatus == CenterStatus.Active);

            if (center == null)
                throw new BusinessException("Center.NotFound");

            using var db = contextFactory.CreateForCenter(centerId);

            var admin = await db.Users
                .FirstOrDefaultAsync(a => a.Id == userId && a.role == userRole.Admin && a.isActive);

            if (admin == null)
                throw new BusinessException("Auth.Forbidden");

            if (request.Year < 0)
                throw new BusinessException("Validation.InvalidYear");

            DateTime periodStart;
            DateTime periodEnd;

            if(request.Month.HasValue)
            {
                if (request.Month.Value < 0 || request.Month.Value > 12)
                    throw new BusinessException("Validation.InvalidMonth");

                periodStart = new DateTime(request.Year, request.Month.Value, 1);
                periodEnd = periodStart.AddMonths(1).AddDays(-1);
            }
            else
            {
                periodStart = new DateTime(request.Year, 1, 1);
                periodEnd = new DateTime(request.Year, 12, 31);
            }

            var confirmedPayments = await db.Payments
                .Include(a => a.Appointment)
                .Include(c => c.Consultation)
                .Include(l => l.LabResult)
                .Where(p => p.Status == PaymentStatus.Paid
                 && p.PaidAt >= periodStart
                 && p.PaidAt <= periodEnd
                 && (
                      (p.AppointmentId != null && p.Appointment!.appointmentStatus == AppointmentStatus.Completed)
                   || (p.ConsultationId != null && p.Consultation!.Status == ConsultationStatus.Completed)
                   || (p.LabResultId != null && p.LabResult!.Status == LabStatus.Completed)
                ))
                .Select(r => new
                {
                    PaymentType = r.Type,
                    r.Amount

                }).ToListAsync();

            using var workbook = new XLWorkbook();
            var paymentSheet = workbook.Worksheets.Add("Payment");
            paymentSheet.Cell(1, 1).Value = "Payment Type";
            paymentSheet.Cell(1, 2).Value = "Amount";
            paymentSheet.Range(1, 1, 1, 2).Style.Font.Bold = true;
            paymentSheet.Range(1, 1, 1, 2).SortLeftToRight();

            var paymentByType = confirmedPayments
                .GroupBy(t => t.PaymentType)
                .Select(t => new { PaymentType = t.Key, Total = t.Sum(x => x.Amount) })
                .ToList();

            int row = 2;
            foreach(var item in paymentByType)
            {
                paymentSheet.Cell(row, 1).Value = item.PaymentType.ToString();
                paymentSheet.Cell(row, 2).Value = item.Total;
                row++;
            }

            paymentSheet.Cell(row, 1).Value = "Amount Total";
            paymentSheet.Cell(row, 1).Style.Font.Bold = true;
            paymentSheet.Cell(row, 2).Value = paymentByType.Sum(x => x.Total);
            paymentSheet.Cell(row, 1).Style.Font.Bold = true;
            paymentSheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();

        }
    }
}
