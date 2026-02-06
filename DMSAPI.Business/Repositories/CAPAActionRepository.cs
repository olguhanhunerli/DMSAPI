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
    public class CAPAActionRepository : GenericRepository<CAPAACTION>, ICAPAActionRepository
    {
        public CAPAActionRepository(DMSDbContext context, IHttpContextAccessor http) : base(context, http)
        {
        }

        public async Task<int> CountNotDoneAsync(string capaNo)
        {
            return await _dbSet.CountAsync(x =>
                x.CapaNo == capaNo);
        }

        public async Task<List<CAPAACTION>> GetByCapaNoAsync(string capaNo)
        {
            return await _dbSet
                .Where(x => x.CapaNo == capaNo)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<CAPAACTION?> GetByIdLongAsync(long id)
        {
           return await _dbSet
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
