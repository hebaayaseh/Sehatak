using Sehatak.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.ServicePriceDto
{
    public class ServicePriceRequest
    {
        public ServiceType Type { get; set; }
        public List<ServicePriceItemDto> Items { get; set; } = new();
    }
}
