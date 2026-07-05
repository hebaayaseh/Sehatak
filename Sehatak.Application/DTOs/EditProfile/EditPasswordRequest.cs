using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.EditProfile
{
    public class EditPasswordRequest
    {
        public string PasswordHash { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
