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
           .Where(x => x.DocumentId == documentId)
           .ToListAsync();
        }
    }
}
