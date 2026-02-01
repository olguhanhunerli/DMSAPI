using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.ComplaintDTO;
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
	public class ComplaintAttachmentRepository : GenericRepository<ComplaintAttachment>, IComplaintAttachmentRepository
	{
		public ComplaintAttachmentRepository(DMSDbContext context, IHttpContextAccessor http) : base(context, http)
		{
		}

		public Task<List<ComplaintAttachment>> GetByComplaintNoAsync(string complaintNo)
		{
			var query = _dbSet
				.AsNoTracking()
				.Where(x => x.ComplaintNo == complaintNo && !x.IsDeleted == true)
				.Include(x => x.UploadedByUser)
				.Include(x => x.DeletedByUser)
				.ToListAsync();
			return query;
		}

		public async Task<ComplaintAttachment?> GetByIdAsync(long id)
		{
			return await _dbSet
			   .AsNoTracking()
			   .Include(x => x.UploadedByUser)
			   .Include(x => x.DeletedByUser)
			   .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted == true);
		}
	}
}
