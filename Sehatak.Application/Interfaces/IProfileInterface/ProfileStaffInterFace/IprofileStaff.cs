using Sehatak.Application.DTOs.EditProfile.EditEmailOrPasswored;
using Sehatak.Application.DTOs.EditProfile.EditProfileActors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.IProfileInterface.ProfileAdmin
{
    public interface IprofileStaff
    {
        Task<EditCenterInformationResponse> EditCenterInformation(int centerId , EditCenterInformationRequest request);
        Task<EditSttafInformationResponse> EditSttafInformation (int centerId , int userId ,  EditSttafInformationRequest request);
        Task<bool> RequestEditEmail(int centerId, int userId, EditEmailRequest request);
        Task<EmailResponse> ConfirmEditEmail(int centerId, int userId, ConfirmEditEmailRequest request);

        Task<bool> RequestEditPassword(int centerId, int userId, EditPasswordRequest request);
        Task<PasswordResponse> ConfirmEditPassword(int centerId, int userId, ConfirmEditPasswordRequest request);
    }
}
