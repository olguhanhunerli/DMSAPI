using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.IRepositories;
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
	public class ActionFilesRepository : GenericRepository<ActionFile>, IActionFilesRepository
	{
		public ActionFilesRepository(DMSDbContext context, IHttpContextAccessor http) : base(context, http)
		{
		}

		public async Task<bool> AnyByActionIdAsync(long actionId)
		{
			return await _dbSet.AnyAsync(x => x.ActionId == actionId);
		}

		public async Task<ActionFile> GetByLongIdAsync(long fileId)
		{
			return await _dbSet
				.FirstOrDefaultAsync(x => x.Id == fileId);
		}
	}
}
