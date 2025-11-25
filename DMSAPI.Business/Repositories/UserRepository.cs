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
        public async Task<User> GetUserByEmailWithRoleAndPermissionsAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.PermissionsList)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
