using Sehatak.Application.DTOs.EditProfile;
using Sehatak.Application.DTOs.SuperAdminProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.ISuperDaminProfile
{
    public interface IProfile
    {
        Task<ProfileResponse> ViewProfile(int superAdminId);

        Task<bool> RequestEditEmail(int superAdminId, EditEmailRequest request);
        Task<EmailResponse> ConfirmEditEmail(int superAdminId, ConfirmEditEmailRequest request);

        Task<bool> RequestEditPassword(int superAdminId, EditPasswordRequest request);
        Task<PasswordResponse> ConfirmEditPassword(int superAdminId, ConfirmEditPasswordRequest request);

        Task<NameResponse> EditName(int superAdminId, EditNameRequest request);
        Task<ProfileImageResponse> EditProfileImage(int superAdminId, EditProfileImageRequest request);
    }
}
