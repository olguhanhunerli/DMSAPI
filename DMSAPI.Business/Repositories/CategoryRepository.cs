using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories
{
    public class CategoryRepository: GenericRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(DMSDbContext context) : base(context)
		{
		}

		public async Task<bool> ExistsAsync(string name, int companyId, int? parentId)
		{
			return await _dbSet
				.AnyAsync(c => c.Name == name && c.CompanyId == companyId && c.ParentId == parentId);
		}

		public async Task<IEnumerable<Category>> GetCategoriesByCompanyId(int companyId)
		{
			return await _dbSet
				.Where(c => c.CompanyId == companyId && !c.IsDeleted)
				.OrderBy(c => c.SortOrder)
				.ToListAsync();
		}

		public Task<Category> GetCategoryWithChildrenAsync(int categoryId)
		{
			return _dbSet
				.Include(c => c.Children.Where(child => !child.IsDeleted))
				.ThenInclude(child => child.Children)
				.Include(c => c.Parent)
				.Include(c => c.Company)
				.FirstOrDefaultAsync(c => c.Id == categoryId && !c.IsDeleted);
		}

		public async Task<IEnumerable<Category>> GetChildrenAsync(int parentId)
		{
			return await _dbSet
				.Where(c => c.ParentId == parentId && !c.IsDeleted)
				.OrderBy(c => c.SortOrder)
				.ToListAsync();
		}
	}
}
