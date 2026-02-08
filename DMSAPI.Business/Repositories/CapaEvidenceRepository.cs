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
	public class CapaEvidenceRepository : GenericRepository<CapaEvidenceFiles>, ICapaEvidenceRepository
	{
		public CapaEvidenceRepository(DMSDbContext context, IHttpContextAccessor http) : base(context, http)
		{
		}

		public async Task<List<CapaEvidenceFiles>> GetFilesAsync(string capaNo)
		{
			var files = await _dbSet.Where(x => x.CapaNo == capaNo).ToListAsync();
			return files ?? new List<CapaEvidenceFiles>();
		}

		public async Task<CapaEvidenceFiles?> GetByIdLongAsync(long id)
		{
			return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
