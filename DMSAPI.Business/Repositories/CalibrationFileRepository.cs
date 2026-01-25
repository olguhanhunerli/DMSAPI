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
	public class CalibrationFileRepository : GenericRepository<DMSAPI.Entities.Models.InstrumentCalibrationFile>, ICalibrationFileRepository
	{
		public CalibrationFileRepository(DMSDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
		{
		}

		public async Task<InstrumentCalibrationFile?> GetCalibrationFileByIdAsync(ulong fileId)
		{
			return await _dbSet.AsNoTracking()
				.FirstOrDefaultAsync(x => x.CompanyId == CompanyId && x.FileId == fileId && x.IsDeleted == false);
		}

		public async Task<PagedResultDTO<InstrumentCalibrationFile>> GetCalibrationFilesByCalibrationIdAsync(int pageNumber, int pageSize)
		{
			if(pageNumber <= 0)
			{
				pageNumber = 1;
			}
			if (pageSize <= 0)
			{
				pageSize = 10;
			}
			var query = _dbSet.AsNoTracking()
				.Where(x => x.CompanyId == CompanyId &&x.IsDeleted == false)
				.OrderByDescending(x => x.CreatedAt);
			var totalRecords = await query.CountAsync();
			var items = await query
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
			return new PagedResultDTO<InstrumentCalibrationFile>
			{
				Items = items,
				TotalCount = totalRecords,
				Page = pageNumber,
				PageSize = pageSize
			};

		}
	}
}
