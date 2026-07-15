using Sehatak.Application.DTOs.EditProfile.EditProfileActors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.IProfileInterface.ProfileDoctor
{
    public interface IProfileDoctor
    {
        Task<EditDoctorInformationResponse> EditDoctorInformation(int centerId, int doctorId, EditDoctorInformationRequest request);
    }
}
