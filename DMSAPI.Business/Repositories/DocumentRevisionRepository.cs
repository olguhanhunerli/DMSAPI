using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
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
	public class DocumentRevisionRepository : GenericRepository<DocumentRevision>, IDocumentRevisionRepository
	{
		public DocumentRevisionRepository(DMSDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
		{
		}

		public async Task<DocumentRevision?> GetActiveByDocumentIdAsync(int documentId)
		{
			return await _dbSet
				.FirstOrDefaultAsync(r =>
				r.DocumentId == documentId &&
				r.IsActive &&
				r.Status == "In Progress"
				);
		}

		public async Task<(List<DocumentRevision>Items, int TotalCount)> GetMyActiveRevisionAsync(int userId, int page, int p)
		{
			var query = _dbSet
				.Include(r => r.Document)
				.ThenInclude(d => d.Category)
				.Where(r =>
					r.IsActive &&
					r.Status == "In Progress" &&
					r.StartedByUserId == userId
				)
				.OrderByDescending(r => r.StartedAt);
			var totalCount = await query.CountAsync();
			var items = await query
				.OrderByDescending(r => r.StartedAt)
				.Skip((page - 1) * p)
				.Take(p)
				.ToListAsync();
			return (items, totalCount);
		}
	}
}
