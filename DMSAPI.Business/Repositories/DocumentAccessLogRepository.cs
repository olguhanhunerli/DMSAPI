using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.DocumentDTOs;
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
    public class DocumentAccessLogRepository : GenericRepository<DocumentAccessLog>, IDocumentAccessLogRepository
    {
        public DocumentAccessLogRepository(DMSDbContext context, IHttpContextAccessor accessor)
        : base(context, accessor) { }
        public async Task<List<DocumentAccessLog>> GetByDocumentIdAsync(int documentId)
        {
            return await _dbSet
             .Include(x => x.User)
            .Where(x => x.DocumentId == documentId)
            .OrderBy(x => x.AccessAt)
            .ToListAsync();
        }

        public async Task<IQueryable<DocumentAccessLog>> GetQueryableAsync(DocumentAccessLogFilterDTO dto)
        {
            var query = _dbSet
                .Include(x => x.User)
                .Include(x => x.Document)
                .AsQueryable();

            if (dto.DocumentId.HasValue)
                query = query.Where(x => x.DocumentId == dto.DocumentId);

            if (dto.UserId.HasValue)
                query = query.Where(x => x.UserId == dto.UserId);

            if (!string.IsNullOrWhiteSpace(dto.AccessType))
                query = query.Where(x => x.AccessType == dto.AccessType);

            if (dto.StartDate.HasValue)
                query = query.Where(x => x.AccessAt >= dto.StartDate.Value);

            if (dto.EndDate.HasValue)
                query = query.Where(x => x.AccessAt <= dto.EndDate.Value);

            return query
                .OrderByDescending(x => x.AccessAt);
        }
    }
}
