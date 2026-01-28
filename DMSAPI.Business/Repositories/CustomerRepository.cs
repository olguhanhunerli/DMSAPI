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
    public class CustomerRepository: GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DMSDbContext context, IHttpContextAccessor accessor) : base(context, accessor)
        {
        }

        public async Task<PagedResultDTO<Customer>> GetAllCustomerAsync(int page, int pageSize)
        {
           if (page <= 0) page = 1;
           if (pageSize <= 0) pageSize = 10;
            var query = _dbSet
                  .Where(x => x.CompanyId == CompanyId && x.IsDelete != true)
                  .Include(x => x.Company)
                  .OrderBy(x => x.Name);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PagedResultDTO<Customer>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public Task<Customer?> GetCustomerByIdAsync(int id)
        {
            var query = _dbSet
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == CompanyId && x.IsDelete != true);
            return query;
        }
    }
}
