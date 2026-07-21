using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.SearchDoctorDto
{
    public class SearchDoctorRequest
    {
        public string? doctorName { get; set; }
        public int? departmentId { get; set; }
        public string? Specialization {  get; set; }
    }
}
