using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.DepartmentDto
{
    public class DepartmentRequestDto
    {
        public string departmentName {  get; set; }
        public string? departmentDescription { get; set; }
        public IFormFile? logo {  get; set; }
    }
}
