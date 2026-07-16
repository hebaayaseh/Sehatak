using Sehatak.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.ServicePriceDto
{
    public class ServicePriceResponse
    {
        public ServiceType Type { get; set; }
        public List<ServicePriceResponseItem> CreatedItems { get; set; } = new();
    }
}
