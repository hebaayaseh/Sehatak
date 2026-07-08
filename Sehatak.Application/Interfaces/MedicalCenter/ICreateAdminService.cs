using Sehatak.Application.DTOs.CreateCenterRequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.MedicalCenter
{
    public interface ICreateAdminService
    {
        Task<CreateAdminResponseDto> CreateAdminAsync(int centerId, CreateAdminRequestDto request);

    }
}
