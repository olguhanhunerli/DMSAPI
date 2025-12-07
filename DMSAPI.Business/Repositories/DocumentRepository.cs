using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
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
