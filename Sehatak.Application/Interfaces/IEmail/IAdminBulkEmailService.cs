using Sehatak.Application.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.IEmail
{
    public interface IAdminBulkEmailService
    {
        Task<int> SendAsync(SendEmailDto request);
    }
}
