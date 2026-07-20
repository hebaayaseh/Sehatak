using Sehatak.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.ServicePriceDto
{
    public class UpaterServicePriceResponse
    {
        public int Id {  get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ServiceType Type { get; set; }
    }
}
