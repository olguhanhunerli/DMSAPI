using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories
{
	public class PositionRepository : GenericRepository<Position>, IPositionRepository
	{
		public PositionRepository(DMSDbContext context, IHttpContextAccessor accessor)
			: base(context, accessor)
		{
		}

		public async Task<IEnumerable<Position>> GetAllPositionsAsync()
		{
			IQueryable<Position> query = _dbSet
				.Include(d => d.Company)
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

        public async Task<PagedResultDTO<Position>> GetPagedAsync(int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

			var baseQuery = _dbSet
				.Include(x => x.CreatedByUser)
				.Include(x => x.UploadedByUser)
				.Include(x => x.Company)
                .Where(x => x.CompanyId == CompanyId); 

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDTO<Position>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = items
            };
        }

        public async Task<Position?> GetPositionByIdAsync(int id)
		{
			var query = _dbSet
				.Include(p => p.Company)
				.Include(p => p.Users)
				.Include(p => p.CreatedByUser)
				.Include(p => p.UploadedByUser)
				.Where(p => p.Id == id);

			if (!IsGlobalAdmin && CompanyId.HasValue)
				query = query.Where(p => p.CompanyId == CompanyId.Value);

			return await query.FirstOrDefaultAsync();
		}
	}
}
