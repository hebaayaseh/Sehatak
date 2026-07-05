using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.EditProfile
{
    public class EditProfileImageRequest
    {
        public IFormFile ImageFile { get; set; }
    }
}
