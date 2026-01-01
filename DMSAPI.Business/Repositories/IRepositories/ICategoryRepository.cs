using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;

namespace DMSAPI.Business.Repositories.IRepositories
{
	public interface ICategoryRepository : IGenericRepository<Category>
	{
		Task<bool> ExistsAsync(string name, int? parentId, int companyId);

		Task<IEnumerable<Category>> GetCategoriesAsync();
		Task<Category?> GetCategoryWithChildrenAsync(int categoryId);
		Task<IEnumerable<Category>> GetChildrenAsync(int parentId);
		Task<int> GetNextSortOrderAsync(int? parentId);

		Task SoftDeleteAsync(int categoryId, int? uploadedBy);
		Task RestoreCategoryAsync(int categoryId, int? uploadedBy);
        Task<PagedResultDTO<Category>> GetPagedAsync(int page, int pageSize);
        Task<List<Category>> GetCategoryTreeWithDocumentsAsync(int companyId);
		Task<Category> GetRootCategoryAsync(int categoryId);

	}
}
