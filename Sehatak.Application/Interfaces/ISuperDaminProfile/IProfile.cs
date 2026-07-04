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
        Task<SuperAdminProfileResponse> ViewProfile(int superAdminId);

    }
}
