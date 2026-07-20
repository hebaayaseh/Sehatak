using Sehatak.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.ServicePriceDto
{
    public class UpdateServicePrice
    {
        public int ServicePriceId { get; set; }
        public string? ServiceName { get; set; } = string.Empty;
        public decimal? Price { get; set; }

    }
}
