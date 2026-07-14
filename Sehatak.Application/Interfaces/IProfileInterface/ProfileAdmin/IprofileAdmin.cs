using Sehatak.Application.DTOs.EditProfile.EditProfileActors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.IProfileInterface.ProfileAdmin
{
    public interface IprofileAdmin
    {
        Task<EditCenterInformationResponse> EditCenterInformation(int centerId , EditCenterInformationRequest request);
        Task<EditSttafInformationResponse> EditSttafInformation (int centerId , int adminId ,  EditSttafInformationRequest request);
    }
}
