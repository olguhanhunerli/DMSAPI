using DMSAPI.Entities.DTOs.CategoryDTOs;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DepartmentDTOs;

namespace DMSAPI.Services.IServices
{
	public interface IDepartmentService
	{
		Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync();

		Task<DepartmentDetailDTO> GetDepartmentByIdAsync(int id);

		Task<DepartmentDetailDTO> CreateDepartmentAsync(
			CreateDepartmentDTO createDepartmentDTO,
			int userIdFromToken
		);

		Task<DepartmentDetailDTO> UpdateDepartmentAsync(
			UpdateDepartmentDTO updateDepartmentDTO,
			int userIdFromToken
		);
		Task DeleteDepartmentAsync(int id, int userId);
        Task<PagedResultDTO<DepartmentDTO>> GetPagedAsync(int page, int pageSize);
    }
}
