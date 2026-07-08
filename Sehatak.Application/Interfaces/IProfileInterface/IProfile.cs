using Sehatak.Application.DTOs.EditProfile;
using Sehatak.Application.DTOs.SuperAdminProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.IProfileInterface
{
    public interface IProfile
    {
        Task<ProfileResponse> ViewProfile(int UserId);

        Task<bool> RequestEditEmail(int UserId, EditEmailRequest request);
        Task<EmailResponse> ConfirmEditEmail(int UserId, ConfirmEditEmailRequest request);

        Task<bool> RequestEditPassword(int UserId, EditPasswordRequest request);
        Task<PasswordResponse> ConfirmEditPassword(int UserId, ConfirmEditPasswordRequest request);

        Task<NameResponse> EditName(int UserId, EditNameRequest request);
        Task<ProfileImageResponse> EditProfileImage(int UserId, EditProfileImageRequest request);
    }
}
