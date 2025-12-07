using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Entities.DTOs.PositionDTOs;

namespace DMSAPI.Services.IServices
{
	public interface IPositionService
	{
		Task<IEnumerable<PositionDTO>> GetAllPositionsAsync();
		Task<PositionDTO> GetPositionByIdAsync(int id);
		Task AddPositionAsync(CreatePositionDTO positionDTO, int userIdFromToken);
		Task<PositionDTO> UpdatePositionAsync(UpdatePositionDTO positionDTO, int userIdFromToken);
	}
}
