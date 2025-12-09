using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories
{
	public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
	{
		public DocumentRepository(DMSDbContext context, IHttpContextAccessor accessor)
			: base(context, accessor)
		{
		}
        private IQueryable<Document> ApplyAccessFilter(
			IQueryable<Document> query,
			int userId,
			int roleId,
			int departmentId)
				{
					return query.Where(d =>
						d.IsPublic
						|| d.CreatedByUserId == userId
						|| (d.AllowedUsers != null &&
							d.AllowedUsers.Contains($"\"{userId}\""))
						|| (d.AllowedRoles != null &&
							d.AllowedRoles.Contains($"\"{roleId}\""))
						|| (d.AllowedDepartments != null &&
							d.AllowedDepartments.Contains($"\"{departmentId}\""))
					);
				}
        public async Task<bool> DocumentCodeExistingAsync(string documentCode)
		{
			return await _dbSet.AnyAsync(d =>
			d.DocumentCode == documentCode &&
			d.CompanyId == CompanyId     
		);
		}

		public async Task<int> GetNextDocumentNumberAsync(int companyId, int categoryId)
		{
			var lastDocCode = await _dbSet
				.Where(d => d.CompanyId == companyId && d.CategoryId == categoryId && !d.IsDeleted)
				.OrderByDescending(d => d.Id)
				.Select(d => d.DocumentCode)
				.FirstOrDefaultAsync();

			if (string.IsNullOrEmpty(lastDocCode))
				return 1;

			var parts = lastDocCode.Split('-');
			var numberPart = parts.Last();

			if (int.TryParse(numberPart, out int number))
				return number + 1;

			return 1;
		}

        public async Task<PagedResultDTO<Document>> GetPagedAuthorizedAsync(
					 int page,
					 int pageSize,
					 int userId,
					 int roleId,
					 int departmentId)
						{
            var query = _dbSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.CompanyId == CompanyId);

            query = ApplyAccessFilter(query, userId, roleId, departmentId);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Company)
                .Include(x => x.Category)
                .ToListAsync();

            return new PagedResultDTO<Document>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = items
            };
        }

        public async Task<bool> ValidateDocumentCodeAsync(string documentCode, int companyId, int categoryId)
		{
			var parts = documentCode.Split('-', 3);
			if (parts.Length != 3)
				return false;

			var companyCode = parts[0];
			var categoryCode = parts[1];
			var numberPart = parts[2];

			var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
			if (company == null || !string.Equals(company.CompanyCode, companyCode, StringComparison.OrdinalIgnoreCase))
				return false;

			var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
			if (category == null || !string.Equals(category.Code, categoryCode, StringComparison.OrdinalIgnoreCase))
				return false;

			if (!int.TryParse(numberPart, out int uniqueNumber))
				return false;

			var existingCount = await _dbSet
				.CountAsync(d => d.CompanyId == companyId && d.CategoryId == categoryId && !d.IsDeleted);

			if (uniqueNumber != existingCount + 1)
				return false;

			return true;
		}
	}
}
