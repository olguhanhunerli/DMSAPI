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
	public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
	{
		public DepartmentRepository(DMSDbContext context, IHttpContextAccessor accessor)
			: base(context, accessor)
		{
		}

		public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
		{
			IQueryable<Department> query = _dbSet
				.Where(d => !d.IsDeleted)
				.Include(d => d.Company)
				.Include(d => d.Manager)
				.Include(d => d.UploadedByUser)
				.Include(d => d.CreatedByUser);

			if (!IsGlobalAdmin && CompanyId.HasValue)
			{
				query = query.Where(d => d.CompanyId == CompanyId.Value);
			}

			return await query
				.AsNoTracking()
				.OrderBy(d => d.Name)
				.ToListAsync();
		}

		public async Task<IEnumerable<Department>> GetDepartmentsByCompanyAsync(int companyId)
		{
			var effectiveCompanyId = IsGlobalAdmin ? companyId : CompanyId;

			if (!effectiveCompanyId.HasValue)
				return Enumerable.Empty<Department>();

			return await _dbSet
				.Where(d => d.CompanyId == effectiveCompanyId.Value && !d.IsDeleted)
				.Include(d => d.Company)
				.Include(d => d.Manager)
				.ToListAsync();
		}

		public async Task<Department> GetDepartmentDetailAsync(int id)
		{
			var query = _dbSet
				.Include(d => d.Company)
				.Include(d => d.Manager)
				.Include(d => d.Users)
				.Where(d => d.Id == id && (d.IsDeleted == false || d.IsDeleted == null));

			if (!IsGlobalAdmin && CompanyId.HasValue)
				query = query.Where(d => d.CompanyId == CompanyId.Value);

			return await query.FirstOrDefaultAsync();
		}

		public async Task<bool> IsNameExistAsync(string name, int companyId)
		{
			return await _dbSet.AnyAsync(d =>
				d.Name == name &&
				d.CompanyId == companyId &&
				!d.IsDeleted
			);
		}

		public async Task<bool> IsCodeExistAsync(string code, int companyId)
		{
			return await _dbSet.AnyAsync(d =>
				d.DepartmentCode == code &&
				d.CompanyId == companyId &&
				!d.IsDeleted
			);
		}

	
	}
}
