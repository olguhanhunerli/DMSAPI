using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DMSAPI.Business.Repositories
{
	public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(DMSDbContext context, IHttpContextAccessor accessor)
			: base(context, accessor) { }

        private IQueryable<Category> CategoryBaseQuery()
        {
            return _dbSet
                .Where(x => !x.IsDeleted)
                .Include(x => x.Company)
                .Include(x => x.Parent)
                .Include(x => x.CreatedByUser)
                .Include(x => x.UpdatedByUser)
                .Include(x => x.Documents); 
        }
        public async Task<bool> ExistsAsync(string name, int? parentId, int companyId)
		{
			return await _dbSet.AnyAsync(c =>
				c.Name == name &&
				c.ParentId == parentId &&
				c.CompanyId == companyId &&
				!c.IsDeleted
			);
		}

		public async Task<IEnumerable<Category>> GetCategoriesAsync()
		{
            return await CategoryBaseQuery()
						.OrderBy(x => x.SortOrder)
						.ToListAsync();
        }

		public async Task<Category?> GetCategoryWithChildrenAsync(int categoryId)
		{
            return await CategoryBaseQuery()
					   .Where(c => c.Id == categoryId)

					   .Include(c => c.Children
						   .Where(child => !child.IsDeleted)
						   .OrderBy(child => child.SortOrder))

						   .ThenInclude(child => child.Documents) 
					   .Include(c => c.Children)
						   .ThenInclude(child => child.Children
							   .Where(gc => !gc.IsDeleted))
						   .ThenInclude(gc => gc.Documents)
					   .FirstOrDefaultAsync();
        }

		public async Task<IEnumerable<Category>> GetChildrenAsync(int parentId)
		{
			return await _dbSet
				.Where(c => c.ParentId == parentId && !c.IsDeleted)
				.Include(c => c.UpdatedByUser)
				.Include(c => c.CreatedByUser)
				.OrderBy(c => c.SortOrder)
				.ToListAsync();
		}

		public async Task<int> GetNextSortOrderAsync(int? parentId)
		{
			return await _dbSet
				.Where(c => c.ParentId == parentId && !c.IsDeleted)
				.CountAsync() + 1;
		}

        public async Task<PagedResultDTO<Category>> GetPagedAsync(int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var baseQuery = _dbSet
                .Where(x => !x.IsDeleted && x.CompanyId == CompanyId) 
                .Include(x => x.Parent)
                .Include(x => x.CreatedByUser)
                .Include(x => x.UpdatedByUser)
                .Include(x => x.Company)
                .OrderBy(x => x.SortOrder);

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDTO<Category>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = items
            };
        }

        public async Task RestoreCategoryAsync(int categoryId, int? uploadedBy)
		{
			var category = await _dbSet.FirstOrDefaultAsync(c => c.Id == categoryId);
			if (category == null)
				throw new Exception("Category not found.");

			category.IsDeleted = false;
			category.UpdatedAt = DateTime.UtcNow;
			category.UpdatedBy = uploadedBy;

			_dbSet.Update(category);
			await _context.SaveChangesAsync();
		}

		public async Task SoftDeleteAsync(int categoryId, int? uploadedBy)
		{
			var category = await _dbSet.FirstOrDefaultAsync(c => c.Id == categoryId);
			if (category == null)
				throw new Exception("Category not found.");

			category.IsDeleted = true;
			category.UpdatedAt = DateTime.UtcNow;
			category.UpdatedBy = uploadedBy;

			_dbSet.Update(category);
			await _context.SaveChangesAsync();
		}

        public async Task<List<Category>> GetCategoryTreeWithDocumentsAsync(int companyId)
        {
            return await _dbSet
				.Where(c => c.CompanyId == companyId && !c.IsDeleted)
				.Include(c => c.Documents.Where(d => !d.IsDeleted))
				.OrderBy(c => c.SortOrder)
				.ToListAsync();
        }

	
	}
}
