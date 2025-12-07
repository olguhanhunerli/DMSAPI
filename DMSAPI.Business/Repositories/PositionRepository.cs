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
