using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.Models;
using Microsoft.AspNetCore.Http;
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
	}
}
