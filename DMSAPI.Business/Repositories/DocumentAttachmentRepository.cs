using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories
{
	public class DocumentAttachmentRepository : GenericRepository<DocumentAttachment>, IDocumentAttachmentRepository
	{
		public DocumentAttachmentRepository(DMSDbContext context, IHttpContextAccessor accessor)
		: base(context, accessor) { }
		public async Task<List<DocumentAttachment>> GetByDocumentIdAsync(int documentId)
		{
			return await _dbSet
		   .Where(x => x.DocumentId == documentId && !x.IsDeleted)
		   .OrderByDescending(x => x.UploadedAt)
		   .ToListAsync();
		}

		public async Task<DocumentAttachment?> GetMainFileAsync(int documentId)
		{
			return await _dbSet
				 .FirstOrDefaultAsync(x =>
				 x.DocumentId == documentId &&
				 x.IsMainFile &&
				 !x.IsDeleted);
		}
		public async Task<DocumentAttachment?> GetByIdAsync(int attachmentId)
		{
			var query = _dbSet
				.AsNoTracking()
				.Include(x => x.Document)
				.Where(x => x.Id == attachmentId && !x.IsDeleted);

			if (!IsGlobalAdmin && CompanyId.HasValue)
			{
				query = query.Where(x => x.Document.CompanyId == CompanyId);
			}

			return await query.FirstOrDefaultAsync();
		}
	}
}
