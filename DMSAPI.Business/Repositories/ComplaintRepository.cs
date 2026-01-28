using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
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
    public class ComplaintRepository : GenericRepository<Complaint>, IComplaintRepository
    {
        public ComplaintRepository(DMSDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
        {
        }
        public async Task<PagedResultDTO<Complaint>> GetAllComplaintsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            var query = _dbSet
                .Where(x => x.CompanyId == CompanyId && x.IsDeleted != true)
                .Include(c => c.Customer)
                .Include(c => c.AssignedToUser)
                .Include(c => c.CreatedByUser)
                .Include(c => c.DeleteByUser)
                .Include(c => c.UpdateByUser)
                .Include(x => x.Company)
                .Include(x => x.ClosedByUser)
                .AsNoTracking();
            var totalRecords = await query.CountAsync();
            var complaints = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PagedResultDTO<Complaint>
            {
                Items = complaints,
                TotalCount = totalRecords,
                Page = pageNumber,
                PageSize = pageSize
            };
        }

        public Task<Complaint?> GetComplaintByIdAsync(long id)
        {
            var query = _dbSet
                .Where(x => x.CompanyId == CompanyId && x.Id == id && x.IsDeleted != true)
                .Include(c => c.Customer)
                .Include(c => c.AssignedToUser)
                .Include(c => c.CreatedByUser)
                .Include(c => c.DeleteByUser)
                .Include(c => c.UpdateByUser)
                .Include(x => x.Company)
                .Include(x => x.ClosedByUser)
                .AsNoTracking();
            return query.FirstOrDefaultAsync();
        }
    }
}
