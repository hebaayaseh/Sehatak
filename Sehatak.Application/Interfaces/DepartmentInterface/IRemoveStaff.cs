using Sehatak.Application.DTOs.DepartmentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.DepartmentInterface
{
    public interface IRemoveStaff
    {
        Task<bool> RemoveStaffAsync(int centerId, RemoveStaffRequestDto request);
        Task<bool> ActiveStaffAsync(int centerId, RemoveStaffRequestDto request);
    }
}
