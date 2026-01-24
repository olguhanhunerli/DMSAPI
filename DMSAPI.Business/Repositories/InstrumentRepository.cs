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
				.Where(x => x.CompanyId == CompanyId && x.IsDeleted == false)
				.Include(x => x.Company)
				.Include(x => x.CreatedByName)
				.Include(x => x.UpdatedByName)
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
		public async Task<Instrument?> GetByIdAsync(int id)
		{
			var instrument = await _dbSet
				.Include(x => x.Company)
				.Include(x => x.CreatedByName)
				.Include(x => x.UpdatedByName)
				.FirstOrDefaultAsync(x => x.Instrument_Id == id && x.CompanyId == CompanyId && x.IsDeleted == false);
			return instrument;
		}

		public async Task<PagedResultDTO<Instrument>> GetDeletedByPagedAsync(int page, int pageSize)
		{
			if (page <= 0) page = 1;
			if (pageSize <= 0) pageSize = 10;

			var query = _dbSet
				.Where(x => x.CompanyId == CompanyId && x.IsDeleted == true)
				.Include(x => x.Company)
				.Include(x => x.CreatedByName)
				.Include(x => x.UpdatedByName)
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

		public async Task<Instrument?> GetDeletedByIdAsync(int id)
		{
			var instrument = await _dbSet
				.Include(x => x.Company)
				.Include(x => x.CreatedByName)
				.Include(x => x.UpdatedByName)
				.FirstOrDefaultAsync(x => x.Instrument_Id == id && x.CompanyId == CompanyId && x.IsDeleted == true);
			return instrument;
		}
	}
}
