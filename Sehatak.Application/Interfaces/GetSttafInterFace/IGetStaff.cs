using Sehatak.Application.DTOs.GetStaffDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.GetSttafInterFace
{
    public interface IGetStaff
    {
        Task<List<GetDoctorsResponseDto>> GetDoctorsAsync(int centerId);
        Task<List<GetReceptionistResponseDto>> GetReceptionistsAsync(int centerId);
        Task<List<GetLapTechnicalDto>> GetLapTechnicalsAsync(int centerId);
        Task<DoctorSummaryDto> GetDoctorAsync(int centerId, int doctorId);
        Task<GetReceptionistResponseDto> GetReceptionistAsync(int centerId, int userId);
        Task<GetLapTechnicalDto> GetLapTechnicalAsync(int centerId, int userId);
    }
}
