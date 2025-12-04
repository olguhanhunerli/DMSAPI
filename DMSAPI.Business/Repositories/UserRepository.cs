using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.UserDTOs;
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
                .Include(u => u.Position)
                .ToListAsync();
        }

		public async Task<IEnumerable<User>> GetEmployeesByManagerIdAsync(int managerId)
		{
			return await _dbSet
                .Where(u => u.ManagerId == managerId)
                .Include(u => u.Company)
				.Include(u => u.Department)
				.Include(u => u.Role)
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
                .Include(u  => u.Position)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

		public async Task<User> GetUserByIdsync(int id)
		{
            return await _dbSet
                .Include(u => u.Role)
                .Include(u => u.Company)
                .Include(u => u.Department)
                .Include(u => u.Manager)
                .Include(u => u.Position)
                .FirstOrDefaultAsync(u => u.Id == id);
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

		public async Task<IEnumerable<User>> SearchUsersAsync(UserSearchDTO userSearchDTO)
		{
			var query = _dbSet
                .Include(u => u.Role)
				.Include(u => u.Company)
				.Include(u => u.Department)
				.Include(u => u.Manager)
				.AsQueryable();
            if(!string.IsNullOrEmpty(userSearchDTO.Keyword))
            {
                query = query.Where(u => u.FirstName.Contains(userSearchDTO.Keyword) ||
                                         u.LastName.Contains(userSearchDTO.Keyword) ||
                                         u.Email.Contains(userSearchDTO.Keyword) ||
                                         u.UserName.Contains(userSearchDTO.Keyword));
			}
            if (userSearchDTO.RoleId.HasValue)
			{
				query = query.Where(u => u.RoleId == userSearchDTO.RoleId.Value);
			}
			if (userSearchDTO.CompanyId.HasValue)
			{
				query = query.Where(u => u.CompanyId == userSearchDTO.CompanyId.Value);
			}
			if (userSearchDTO.DepartmentId.HasValue)
			{
				query = query.Where(u => u.DepartmentId == userSearchDTO.DepartmentId.Value);
			}
			if (userSearchDTO.IsActive.HasValue)
			{
				query = query.Where(u => u.IsActive == userSearchDTO.IsActive.Value);
			}
            return await query.ToListAsync();
		}
	}
}
