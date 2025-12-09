using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Entities.DTOs.RoleDTOs;

namespace DMSAPI.Services.IServices
{
	public interface IRoleService
	{
		Task<IEnumerable<RoleDTO>> GetAllRoleAsync();

		Task<RoleDTO> GetByIdAsync(int id);

		Task AddAsync(
			AddRoleDTO roleDTO,
			int userIdFromToken
		);

		Task UpdateAsync(
			int id,
			UpdateRoleDTO roleDTO,
			int userIdFromToken
		);

		Task DeleteAsync(
			int id,
			int userIdFromToken
		);
        Task<PagedResultDTO<RoleDTO>> GetPagedAsync(int page, int pageSize);
    }
}
