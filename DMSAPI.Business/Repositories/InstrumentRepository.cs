using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories
{
	public class InstrumentRepository : GenericRepository<Instrument>, IInstrumentRepository
	{
		public InstrumentRepository(DMSDbContext context, IHttpContextAccessor accessor)
			: base(context, accessor) { }

		public async Task<PagedResultDTO<Instrument>> GetPagedAsync(int page, int pageSize)
		{
			if(page <= 0) page = 1;
			if(pageSize <= 0) pageSize = 10;

			var query = _dbSet
				.Where(x => x.CompanyId == CompanyId)
				.Include(x => x.Company)
				.OrderBy(x => x.Asset_Code);
			var totalCount = await query.CountAsync();
			var items = await query
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
			return new PagedResultDTO<Instrument>
			{
				Items = items,
				TotalCount = totalCount,
				Page = page,
				PageSize = pageSize
			};
		
		}
	}
}
