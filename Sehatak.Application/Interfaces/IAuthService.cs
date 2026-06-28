using Sehatak.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterRequestDto> RegisterAsync(RegisterRequestDto request);
        Task<VerifyOtpResponseDto> VerifyOtpAsync(VerifyOtpRequestDto request);
    }
}
