using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;


namespace DMSAPI.Business.Repositories
{
	public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
	{
		public DocumentRepository(DMSDbContext context) : base(context)
		{
		}

		public async Task<bool> DocumentCodeExistingAsync(string documentCode)
		{
			return await _dbSet.AnyAsync(d => d.DocumentCode == documentCode);
		}

		public async Task<int> GetNextDocumentNumberAsync(int companyId, int categoryId)
		{
			var lastDoc = await _context.Documents
			   .Where(d => d.CompanyId == companyId && d.CategoryId == categoryId)
			   .OrderByDescending(d => d.Id)
			   .Select(d => d.DocumentCode)
			   .FirstOrDefaultAsync();

			if (string.IsNullOrEmpty(lastDoc))
				return 1;

			var parts = lastDoc.Split('-');
			var numberPart = parts.Last();     

			if (int.TryParse(numberPart, out int number))
				return number + 1;

			return 1;
		}

		public async Task<bool> ValidateDocumentCodeAsync(string documentCode, int companyId, int categoryId)
		{
			var parts = documentCode.Split('-', 2);
			if (parts.Length < 2)
			{
				return await Task.FromResult(false); 
			}

			var codeParts = parts[0].Split('-');
			if(codeParts.Length < 3)
			{
				return await Task.FromResult(false); 
			}
			string companyCode = codeParts[0];
			string categoryCode = codeParts[1];
			string uniqueIdentifier = codeParts[2];

			var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
			if (company == null || company.CompanyCode != companyCode)
			{
				return false;
			}
			var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
			if (category == null || category.Code != categoryCode)
			{
				return false;
			}
			if (!int.TryParse(uniqueIdentifier, out int uniqueIdentifierNumber))
			{
				return false;
			}
			int existingCount = await _context.Documents
				.CountAsync(d => d.CategoryId == categoryId && !d.IsDeleted);
			if (uniqueIdentifierNumber != existingCount + 1)
				return false;
			return true;
		}
	}
}
