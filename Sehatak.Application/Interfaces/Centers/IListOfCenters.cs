using Sehatak.Application.DTOs.CentersDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Application.Interfaces.Centers
{
    public interface IListOfCenters
    {
        Task<List<ListOfCentersResponse>> GetListOfCenters();
    }
}
