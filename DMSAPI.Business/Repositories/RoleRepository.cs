using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories
{
	public class RoleRepository : GenericRepository<Role>, IRoleRepository
	{
		public RoleRepository(DMSDbContext context, IHttpContextAccessor accessor)
			: base(context, accessor)
		{
		}

		public async Task<IEnumerable<Role>> GetAllRolesAsync()
		{
			return await _dbSet
                .Where(x => x.CompanyId == CompanyId)
				.Include(r => r.CreatedByUser)
				.Include(r => r.UploadedByUser)
				.ToListAsync();
		}

        public async Task<PagedResultDTO<Role>> GetPagedAsync(int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var baseQuery = _dbSet
                .Where(x => x.CompanyId == CompanyId)

                .Include(x => x.CreatedByUser)
                .Include(x => x.UploadedByUser);

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDTO<Role>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = items
            }; throw new NotImplementedException();
        }
    }
}
