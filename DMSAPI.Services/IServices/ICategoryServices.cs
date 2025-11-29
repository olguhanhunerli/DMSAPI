using DMSAPI.Entities.DTOs.CategoryDTOs;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface ICategoryServices
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
		Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO categoryDto);
        Task<IEnumerable<CategoryDTO>> GetCategoriesByCompanyIdAsync(int companyId);
		Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> UpdateCategoryByIdAsync(UpdateCategoryDTO categoryDto);
        Task<IEnumerable<CategoryTreeDTO>> GetCategoryTreeAsync(int companyId);
        Task<IEnumerable<CategoryDTO>> SearchCategoryTreeAsync(string keyword, int companyId);
	}
}
