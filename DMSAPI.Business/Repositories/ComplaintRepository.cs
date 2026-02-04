using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
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
                .Include(c => c.CreatedByUser)
                .Include(c => c.DeleteByUser)
                .Include(c => c.UpdateByUser)
                .Include(x => x.Company)
                .Include(x => x.ClosedByUser)
                .Include(c => c.Assignees)
                    .ThenInclude(a => a.User)
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

        public Task<Complaint?> GetByComplaintNoAsync(string complaintNo)
        {
            return _dbSet
          .Where(x =>
              x.CompanyId == CompanyId &&
              x.ComplaintNo == complaintNo &&
              x.IsDeleted != true)
          .Include(c => c.Customer)
          .Include(c => c.CreatedByUser)
          .Include(c => c.DeleteByUser)
          .Include(c => c.UpdateByUser)
          .Include(c => c.Company)
          .Include(c => c.ClosedByUser)
          .Include(c => c.Assignees)
            .ThenInclude(a => a.User)
          .AsNoTracking()
          .FirstOrDefaultAsync();
        }

        public async Task<List<ComplaintForCapaSelectDTO?>> GetForCapaSelectAsync(int companyId, string? search, int take)
        {
            var q = _dbSet.AsNoTracking()
                 .Where(x => x.CompanyId == companyId && x.NeedsCapa && !x.IsDeleted && !x.IsClosed && !x.IsCapa);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(x =>
                    x.ComplaintNo.Contains(s) ||
                    x.Title.Contains(s) ||
                    (x.Customer != null && x.Customer.Name.Contains(s)));
            }

            return await q
                .OrderByDescending(x => x.ReportedAt)
                .Select(x => new ComplaintForCapaSelectDTO
                {
                    Id = x.Id,
                    ComplaintNo = x.ComplaintNo,
                    Title = x.Title,
                    CustomerName = x.Customer != null ? x.Customer.Name : null,
                    SeverityId = x.SeverityId,
                    ReportedAt = x.ReportedAt
                })
                .Take(take)
                .ToListAsync();
        }
    }
}
