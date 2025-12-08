using DMSAPI.Entities.DTOs.CategoryDTOs;
using DMSAPI.Entities.DTOs.Common;

namespace DMSAPI.Services.IServices
{
	public interface ICategoryServices
	{
		Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();

		Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO categoryDto);

		Task<CategoryDTO> GetCategoryByIdAsync(int id);

		Task<CategoryDTO> UpdateCategoryByIdAsync(UpdateCategoryDTO categoryDto, int userIdFromToken);

		Task<IEnumerable<CategoryTreeDTO>> GetCategoryTreeAsync(int companyId);

		Task<IEnumerable<CategoryDTO>> SearchCategoryTreeAsync(string keyword, int companyId);

		Task<bool> SoftDeleteCategoryAsync(CategoryDeleteDTO categoryDeleteDTO, int userIdFromToken);

		Task<bool> RestoreCategoryAsync(CategoryRestoreDTO categoryRestoreDTO);

		Task<List<string>> GetCategoryBreadcrumbAsync(int categoryId);

		Task<CategoryBreadcrumbDTO> GetCategoryBreadcrumbDetailedAsync(int categoryId);

		Task<IEnumerable<CategorySelectListDTO>> GetCategorySelectListAsync();
        Task<PagedResultDTO<CategoryDTO>> GetPagedAsync(int page, int pageSize);
    }
}
