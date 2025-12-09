using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;

namespace DMSAPI.Business.Repositories.IRepositories
{
	public interface IDepartmentRepository : IGenericRepository<Department>
	{
		Task<IEnumerable<Department>> GetAllDepartmentsAsync();
		Task<IEnumerable<Department>> GetDepartmentsByCompanyAsync(int companyId);

		Task<Department?> GetDepartmentDetailAsync(int id);

		Task<bool> IsNameExistAsync(string name, int companyId);
		Task<bool> IsCodeExistAsync(string code, int companyId);
        Task<PagedResultDTO<Department>> GetPagedAsync(int page, int pageSize);
    }
}
