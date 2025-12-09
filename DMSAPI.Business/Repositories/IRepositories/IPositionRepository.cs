using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.PositionDTOs;
using DMSAPI.Entities.Models;

namespace DMSAPI.Business.Repositories.IRepositories
{
	public interface IPositionRepository : IGenericRepository<Position>
	{
		Task<IEnumerable<Position>> GetAllPositionsAsync();
		Task<Position?> GetPositionByIdAsync(int id);
        Task<PagedResultDTO<Position>> GetPagedAsync(int page, int pageSize);
    }
}
