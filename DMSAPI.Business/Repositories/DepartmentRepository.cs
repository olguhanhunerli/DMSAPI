using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
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
                .Where(d => !d.IsDeleted && d.CompanyId == CompanyId)
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

        public async Task<PagedResultDTO<Department>> GetPagedAsync(int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var baseQuery = _dbSet
                .Where(x => !x.IsDeleted && x.CompanyId == CompanyId)
                .Include(x => x.CreatedByUser)
                .Include(x => x.UploadedByUser)
                .Include(x => x.Company)
                .OrderBy(x => x.SortOrder);

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDTO<Department>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = items
            };
        }

        public async Task<List<Department>> GetByIdsAsync(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return new List<Department>();

            return await _dbSet
                .AsNoTracking()
                .Where(d =>
                    ids.Contains(d.Id) &&
                    !d.IsDeleted &&
                    d.CompanyId == CompanyId)
                 .Include(d => d.Manager)
				.Include(d => d.Company)
                .ToListAsync();
        }
    }
}
