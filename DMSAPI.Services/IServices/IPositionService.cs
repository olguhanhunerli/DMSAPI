using DMSAPI.Entities.DTOs.PositionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IPositionService
    {
        Task<IEnumerable<PositionDTO>> GetAllPositionsAsync();
    }
}
