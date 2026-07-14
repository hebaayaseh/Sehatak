using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.EditProfile.EditProfileActors
{
    public class EditDoctorInformationRequest
    {
        public string? firstNmae { get; set; }
        public string? lastNmae { get; set; }
        public IFormFile? profileImage { get; set; }
        public string? address { get; set; }
        public string? city { get; set; }
        public string? phoneNumber { get; set; }
        public string? Bio {  get; set; }
        public string? Specialization { get; set; }
    }
}
