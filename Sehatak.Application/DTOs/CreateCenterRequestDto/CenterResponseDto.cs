using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.CreateCenterRequestDto
{
    public class CenterResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UniqueUrl { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<string> EnabledFeatures { get; set; } = new();
    }
}
