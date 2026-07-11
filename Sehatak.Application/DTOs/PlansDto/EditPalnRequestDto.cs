using Sehatak.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.PlansDto
{
    public class EditPalnRequestDto
    {
        public string? name { get; set; }
        public decimal? price { get; set; }
        public int? DurationDays { get; set; }
    }
}
