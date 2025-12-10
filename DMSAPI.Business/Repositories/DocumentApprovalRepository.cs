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
    public class DocumentApprovalRepository : GenericRepository<DocumentApproval>, IDocumentApprovalRepository
    {
        public DocumentApprovalRepository(DMSDbContext context, IHttpContextAccessor accessor)
        : base(context, accessor) { }
        public async Task<List<DocumentApproval>> GetByDocumentIdAsync(int documentId)
        {
            return await _dbSet
                 .Where(x => x.DocumentId == documentId)
                 .OrderBy(x => x.ApprovalLevel)
                 .ToListAsync();
        }

        public async Task<DocumentApproval> GetNextPendingApprovalAsync(int documentId)
        {
            return await _dbSet
             .Where(x =>
                 x.DocumentId == documentId &&
                 !x.IsApproved &&
                 !x.IsRejected)
             .OrderBy(x => x.ApprovalLevel)
             .FirstOrDefaultAsync();
        }

        public async Task<List<int>> GetPendingDocumentIdsAsync(int userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId &&
                       !x.IsApproved  &&
                       !x.IsRejected)
                .Select(x => x.DocumentId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<int>> GetPendingDocumentIdsForUserAsync(int userId)
        {
            return await _context.DocumentApprovals
            .AsNoTracking()
            .Where(x =>
                x.UserId == userId &&
                !x.IsApproved &&
                !x.IsRejected)
            .Select(x => x.DocumentId)
            .Distinct()
            .ToListAsync();
        }
    }
}
