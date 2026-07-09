using Sehatak.Application.DTOs.FinancialReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.IFinancialReports
{
    public interface IFinancialReport
    {
        Task<byte[]> GenerateReportAsync(FinancialReportRequestDto request);

    }
}
