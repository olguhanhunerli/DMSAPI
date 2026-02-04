using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.ComplaintDTO;
using DMSAPI.Entities.DTOs.CustomerDTO;
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
    public class CAPARepository : GenericRepository<CAPA>, ICAPARepository
    {
        public CAPARepository(DMSDbContext context, IHttpContextAccessor http) : base(context, http)
        {
        }

        public async Task<bool> ComplaintExistsAsync(string complaintNo)
        {
            return await _context.Capas
             .AsNoTracking()
             .AnyAsync(x => x.ComplaintNo == complaintNo);
        }

        public async Task<PagedResultDTO<CAPA>> GetAllCAPAAsync(int page, int pageSize)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            var query = _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == CompanyId && !x.IsClosed)
                .Include(x => x.OwnerByUser)
                .Include(x => x.EffectivenessCheckedByUser)
                .Include(x => x.Complaints)
                .Include(x => x.RootCauseMethod)
                .OrderByDescending(x => x.CreatedAt);

            var totalRecords = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDTO<CAPA>
            {
                Items = items,
                TotalCount = totalRecords,
                Page = page,
                PageSize = pageSize
            };
        }

        public Task<CAPA> GetCAPAByCapaNoAsync(string capaNo)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == CompanyId && !x.IsClosed)
                .Include(x => x.OwnerByUser)
                .Include(x => x.EffectivenessCheckedByUser)
                .Include(x => x.Complaints)
                .Include(x => x.RootCauseMethod)
                .FirstOrDefaultAsync();
            return query;
        }

        public Task<string?> GetCompanyCodeAsync(int companyId)
        {
            return _context.Companies
               .Where(x => x.Id == companyId)
               .Select(x => x.CompanyCode)
               .FirstOrDefaultAsync();
        }

        public async Task<ComplaintDTO?> GetComplaintDtoByNoAsync(string complaintNo)
        {
            return await _context.Complaints
            .AsNoTracking()
            .Where(x => x.CompanyId == CompanyId && x.ComplaintNo == complaintNo && !x.IsDeleted)
            .Select(x => new ComplaintDTO
            {
                Id = x.Id,
                ComplaintNo = x.ComplaintNo,

                CompanyName = x.Company != null ? x.Company.Name : null!,
                CustomerId = x.CustomerId,
                CustomerName = x.Customer != null ? x.Customer.Name : null,

                ChannelId = x.ChannelId,
                TypeId = x.TypeId,
                SeverityId = x.SeverityId,

                Title = x.Title,
                Description = x.Description,
                PartNumber = x.PartNumber,
                PartRevision = x.PartRevision,
                LotNumber = x.LotNumber,
                SerialNumber = x.SerialNumber,
                ProductionDate = x.ProductionDate,
                ProductionLine = x.ProductionLine,

                CustomerComplaintNo = x.CustomerComplaintNo,
                CustomerPO = x.CustomerPO,
                DeliveryNoteNo = x.DeliveryNoteNo,

                QuantityAffected = x.QuantityAffected,
                ContainmentAction = x.ContainmentAction,

                IsRepeat = x.IsRepeat,
                NeedsCapa = x.NeedsCapa,
                InterimActionRequired = x.InterimActionRequired,
                InterimActionNote = x.InterimActionNote,

                IsClosed = x.IsClosed,
                IsDeleted = x.IsDeleted,
                Status = x.Status,

                ReportedAt = x.ReportedAt,

                CreatedBy = x.CreatedBy,
                CreatedByName = x.CreatedByUser != null ? x.CreatedByUser.FirstName + " " + x.CreatedByUser.LastName : null,
                ClosedByName = x.ClosedByUser != null ? x.ClosedByUser.FirstName + " " + x.ClosedByUser.LastName : null,

                DeletedBy = x.DeletedBy,
                DeletedByName = x.DeleteByUser != null ? x.DeleteByUser.FirstName + " " + x.DeleteByUser.LastName : null,

                UpdateBy = x.UpdateBy,
                UpdateByName = x.UpdateByUser != null ? x.UpdateByUser.FirstName + " " + x.UpdateByUser.LastName : null,

                ClosedAt = x.ClosedAt,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt
            })
            .FirstOrDefaultAsync();
        }

        public async Task<CustomerMiniDTO?> GetCustomerMiniByIdAsync(int customerId)
        {
            return await _context.Customers
            .AsNoTracking()
            .Where(x => x.Id == customerId  && x.CompanyId == CompanyId)
            .Select(x => new CustomerMiniDTO
            {
                Id = x.Id,
                Name = x.Name,
            })
            .FirstOrDefaultAsync();
        }
         
        public async Task<List<LookupItemDTO>> GetRootCauseMethodLookupsAsync()
        {
            return await _context.root_cause_methods
             .AsNoTracking()
             .Where(x => x.IsActive)
             .OrderBy(x => x.Id)
             .Select(x => new LookupItemDTO
             {
                 Id = x.Id,
                 Code = x.Code,
                 Text = x.NameTr
             })
             .ToListAsync();

        }

        public async Task<List<RootCauseMethod>> GetRootCouseMethodAsync()
        {
            return await _context.root_cause_methods
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Id)
                .ToListAsync();

        }

        public Task<bool> RootCauseMethodExistsAsync(int id)
        {
            return  _context.root_cause_methods.AnyAsync(x => x.Id == id);
        }
        public Task<string?> GetCompanyNameByIdAsync(int companyId)
        {
            return _context.Companies
                .AsNoTracking()
                .Where(x => x.Id == companyId)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();
        }

        public Task<string?> GetUserFullNameByIdAsync(int userId)
        {
            return _context.Users
                .AsNoTracking()
                .Where(x => x.Id == userId)
                .Select(x => x.FirstName + " " + x.LastName)
                .FirstOrDefaultAsync();
        }
    }
}
