using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;

namespace DMSAPI.Business.Repositories.IRepositories
{
	public interface IRoleRepository : IGenericRepository<Role>
	{
		Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<PagedResultDTO<Role>> GetPagedAsync(int page, int pageSize);
    }
}
