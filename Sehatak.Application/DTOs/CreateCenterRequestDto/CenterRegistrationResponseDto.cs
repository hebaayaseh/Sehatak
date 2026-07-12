using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.DTOs.CreateCenterRequestDto
{
    public class CenterRegistrationResponseDto
    {
        public int Id { get; set; }
        public string CenterName { get; set; } = string.Empty;
        public string CenterAddress { get; set; } = string.Empty;
        public string CenterPhone { get; set; } = string.Empty;
        public int PlanId { get; set; }
        public string AdminFirstName { get; set; } = string.Empty;
        public string AdminLastName { get; set; } = string.Empty;
        public string AdminEmail { get; set; } = string.Empty;
        public string AdminPhone { get; set; } = string.Empty;
        public CenterRegistrationStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? RejectionReason { get; set; }
        public int? CreatedCenterId { get; set; }


    }
}
