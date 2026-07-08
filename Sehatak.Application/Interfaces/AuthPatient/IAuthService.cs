using Sehatak.Application.DTOs.PatienRegisterDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.AuthPatient
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(int centerId,RegisterRequestDto request);
        Task<VerifyOtpResponseDto> VerifyOtpAsync(int centerId, VerifyOtpRequestDto request);
    }
}
