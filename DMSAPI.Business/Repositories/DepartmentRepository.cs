using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories
{
    public class DepartmentRepository: GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(DMSDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _dbSet
                .Where(d => !d.IsDeleted)
                .Include(d => d.Company)
                .Include(d => d.Manager)
                .ToListAsync();
        }
        public async Task<IEnumerable<Department>> GetDepartmentsByCompanyAsync(int companyId)
        {
            return await _dbSet
                .Where(d => d.CompanyId == companyId && !d.IsDeleted)
                .Include(d => d.Company)
                .Include(d => d.Manager)
                .ToListAsync();
        }
        public async Task<Department> GetDepartmentDetailAsync(int id)
        {
            return await _dbSet
                .Include(d => d.Company)
                .Include(d => d.Manager)
                .Include(d => d.Users)
                .FirstOrDefaultAsync(d => d.Id == id && (d.IsDeleted == false || d.IsDeleted == null));
        }

        public async Task<bool> IsNameExistAsync(string name, int companyId)
        {
            return await _dbSet.AnyAsync(d =>
                d.Name == name && d.CompanyId == companyId && !d.IsDeleted
            );
        }

        public async Task<bool> IsCodeExistAsync(string code, int companyId)
        {
            return await _dbSet.AnyAsync(d =>
                d.DepartmentCode == code && d.CompanyId == companyId && !d.IsDeleted
            );
        }
    }
}
