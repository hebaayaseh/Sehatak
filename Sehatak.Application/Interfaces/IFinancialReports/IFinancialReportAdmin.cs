using Sehatak.Application.DTOs.FinancialReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.IFinancialReports
{
    public interface IFinancialReportAdmin
    {
        Task<byte[]> GenerateReportAsync(int centerId , int userId , FinancialReportRequestDto request);
    }
}
