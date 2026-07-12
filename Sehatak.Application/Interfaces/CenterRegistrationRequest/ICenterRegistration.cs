using Sehatak.Application.DTOs.CreateCenterRequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.CenterRegistrationRequest
{
    public interface ICenterRegistration
    {
        Task<CenterRegistrationResponseDto> CenterRequestAsync(CenterRegistrationRequestDto request);
        Task<List<CenterRegistrationResponseDto>> GetCentersRegisteration();
        Task<CenterRegistrationResponseDto> GetCenterRegistrationAsync(int requestId);
        Task<bool> ApproveCenterRequest(int requestId, int superAdminId);
        Task<bool> RejectAsync(int requestId, int superAdminId, string rejectionReason);
    }
}
