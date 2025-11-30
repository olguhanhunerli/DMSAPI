using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface ICategoryRepository: IGenericRepository<Category>
	{
		Task<IEnumerable<Category>> GetCategoriesByCompanyId(int companyId);
		Task<IEnumerable<Category>> GetChildrenAsync(int parentId);
		Task<Category> GetCategoryWithChildrenAsync(int categoryId);
		Task<bool> ExistsAsync(string name, int companyId, int? parentId);
		Task SoftDeleteAsync(int categoryId, int? uploadedBy);
		Task RestoreCategoryAsync(int categoryId, int? uploadedBy);
		Task<int> GetNextSortOrder(int companyId, int? parentId);
	}
}
