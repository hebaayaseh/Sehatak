using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.ChatHubDto
{
    public class UserSearchResultDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }
    }
}
