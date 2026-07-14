using Sehatak.Application.DTOs.PatientCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.IPatientCenter
{
    public interface IGetpatientCenter
    {
        Task<List<GetPatientResponseDto>> GetPatientesAsync(int centerId);
        Task<GetPatientResponseDto> GetPatientAsync(int centerId , GetPatientRequestDto request);
    }
}
