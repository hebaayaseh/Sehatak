using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.DepartmentDto
{
    public class DepartmentUpdateRequestDto
    {
        public int departmentId { get; set; }
        public string? departmentName {  get; set; }
        public string? departmentdiscription { get; set; }
        public IFormFile? logo {  get; set; }

    }
}
