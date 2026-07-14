using Sehatak.Application.DTOs.StaffSignup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.SignUp
{
    public interface ISignup
    {
        Task<ReceptionistResponseDto> AddReceptionistAsync(int centerId , ReceptionistRequestDto request);
        Task<LapTechnicalResponseDto> AddLapTechnicalAsync(int centerId , LapTechnicalRequestDto request);
    }
}
