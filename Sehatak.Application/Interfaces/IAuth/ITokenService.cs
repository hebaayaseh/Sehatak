using Sehatak.Application.DTOs.Auth;
using Sehatak.Domain.Enums.SharedEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.IAuth
{
    public interface ITokenService
    {
        Task<TokenResponseDto> IssueTokensAsync(int userId, string name, string email, string role, int? centerId, TokenOwnerType ownerType);
        Task<TokenResponseDto> RefreshAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}
