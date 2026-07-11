using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.PlansDto
{
    public class EditRespondeDto
    {
        public int Id { get; set; }
        public string? name {  get; set; }
        public int? DurationDays { get; set; }
        public decimal? price { get; set; }
    }
}
