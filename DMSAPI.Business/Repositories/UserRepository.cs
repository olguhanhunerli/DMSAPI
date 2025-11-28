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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DMSDbContext context) : base(context) { }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.Company)
                .Include(u => u.Department)
                .Include(u => u.Manager)
                .ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.Company)
                .Include(u => u.Department)
                .Include(u => u.Manager)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByEmailWithRoleAndPermissionsAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.Company)
                .Include(u => u.Department)
                .Include(u => u.Manager)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserWithRelationsAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.Company)
                .Include(u => u.Department)
                .Include(u => u.Manager)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
