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
    }
}
