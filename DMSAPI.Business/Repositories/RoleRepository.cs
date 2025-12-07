using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
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
				.Include(r => r.CreatedByUser)
				.Include(r => r.UploadedByUser)
				.ToListAsync();
		}
	}
}
