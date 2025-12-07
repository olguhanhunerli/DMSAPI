using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories
{
	public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
	{
		public CompanyRepository(DMSDbContext context, IHttpContextAccessor accessor)
			: base(context, accessor)
		{
		}

		public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
		{
			if (IsGlobalAdmin)
			{
				return await _dbSet.ToListAsync();
			}

			if (CompanyId.HasValue)
			{
				return await _dbSet
					.Where(c => c.Id == CompanyId.Value)
					.ToListAsync();
			}

			return Enumerable.Empty<Company>();
		}

		public override async Task<Company> GetByIdAsync(int id)
		{
			var company = await _dbSet.FirstOrDefaultAsync(c => c.Id == id);

			if (company == null)
				return null;

			if (!IsGlobalAdmin && CompanyId.HasValue && company.Id != CompanyId.Value)
				return null;

			return company;
		}
	}
}
