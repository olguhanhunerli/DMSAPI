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
				 .ThenInclude(child => child.Children.Where(gc => !gc.IsDeleted))
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

		public async Task<int> GetNextSortOrder(int companyId, int? parentId)
		{
			return await _dbSet
				.Where(c => c.CompanyId == companyId && c.ParentId == parentId && !c.IsDeleted)
				.CountAsync() + 1;
		}

		public async Task RestoreCategoryAsync(int categoryId, int? uploadedBy)
		{
			var category = await _dbSet.FirstOrDefaultAsync(c => c.Id == categoryId);
			if (category == null)
			{
				throw new Exception("Category not found.");
			}
			category.IsDeleted = false;
			category.UpdatedAt = DateTime.UtcNow;
			category.UpdatedBy = uploadedBy;
			_dbSet.Update(category);
			await _context.SaveChangesAsync();
		}

		public async Task SoftDeleteAsync(int categoryId, int? uploadedBy)
		{
			var category = await _dbSet.FirstOrDefaultAsync(c => c.Id == categoryId);
			if(category == null)
			{
				throw new Exception("Category not found.");
			}
			category.IsDeleted = true;
			category.UpdatedAt = DateTime.UtcNow;
			category.UpdatedBy = uploadedBy;
			_dbSet.Update(category);
			await _context.SaveChangesAsync();
		}
	}
}
