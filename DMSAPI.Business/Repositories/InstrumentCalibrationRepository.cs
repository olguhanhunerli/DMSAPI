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
	public class InstrumentCalibrationRepository : GenericRepository<InstrumentCalibration>, IInstrumentCalibrationRepository
	{
		public InstrumentCalibrationRepository(DMSDbContext context, IHttpContextAccessor accessor)
			: base(context, accessor) { }

		public async Task<PagedResultDTO<InstrumentCalibration>> GetInstrumentCalibrationsAsync(int pageNumber, int pageSize)
		{
			if (pageNumber <= 0) pageNumber = 1;
			if (pageSize <= 0) pageSize = 10;
			var query = _dbSet
				.AsNoTracking()
				.Where(x => x.CompanyId == CompanyId && x.IsDeleted == false)
				.Include(x => x.InstrumentName)
				.Include(x => x.CompanyName)
				.Include(x => x.CreatedByName)
				.Include(x => x.UpdatedByName)
				.Include(x => x.Files.Where(x => x.IsDeleted == false))
				.ThenInclude(x => x.CreatedByName)
                .Include(x => x.Files.Where(x => x.IsDeleted == false))
				.ThenInclude(x => x.UpdatedByName)
                .OrderByDescending(x => x.CalibrationDate);
			var totalCount = await query.CountAsync();
			var items = await query
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
			return new PagedResultDTO<InstrumentCalibration>
			{
				Items = items,
				TotalCount = totalCount,
				Page = pageNumber,
				PageSize = pageSize
			};
		}
		public async Task<InstrumentCalibration?> GetByIdAsync(ulong id)
		{
			var calibration = await _dbSet
				.AsNoTracking()
				.Include(x => x.InstrumentName)
				.Include(x => x.CompanyName)
				.Include(x => x.CreatedByName)
				.Include(x => x.UpdatedByName)
				.Include(x => x.Files.Where(x => x.IsDeleted == false))
				.ThenInclude(x => x.UpdatedByName)
				.Include(x => x.Files.Where(x => x.IsDeleted == false))
				.ThenInclude (x => x.CreatedByName)
				.FirstOrDefaultAsync(x => x.CalibrationId == (ulong)id && x.CompanyId == CompanyId && x.IsDeleted == false);
			return calibration;
		}

		public async Task<InstrumentCalibration?> GetForUpdateAsync(ulong id)
		{
			return await _dbSet
			   .FirstOrDefaultAsync(x =>
				   x.CalibrationId == id &&
				   x.CompanyId == CompanyId &&
				   x.IsDeleted == false);
		}
	}
}
