using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.DTOs.SuperAdminProfile;
using Sehatak.Application.Interfaces.ISuperDaminProfile;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.SuperAdminAuth
{
    public class SuperAdminProfileService : IProfile
    {
        public readonly SharedDbContext sharedDbContext;
        public SuperAdminProfileService(SharedDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public async Task<SuperAdminProfileResponse> ViewProfile(int superAdminId)
        {
            var superAdmin = await sharedDbContext.SuperAdmins.FindAsync(superAdminId);
            if (superAdmin == null)
                throw new BusinessException("Auth.Unauthorized");
            string? Image = null;
            if (!string.IsNullOrEmpty(superAdmin.ProfileImageUrl))
            {
                Image = superAdmin.ProfileImageUrl;
            }

            return new SuperAdminProfileResponse
            {
                Name = superAdmin.Name,
                Email = superAdmin.Email,
                Phone = superAdmin.phone,
                ProfileImage = Image
            };

        }
    }
}
