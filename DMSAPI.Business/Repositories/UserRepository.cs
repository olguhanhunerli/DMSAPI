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
                .Where(u => u.IsDeleted == false ||u.IsDeleted == null)
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

        public async Task<(IEnumerable<User> Users, int TotalCount)> SearchUsersAsync(UserSearchDTO dto)
        {
            var query = _dbSet
                .Where(u => u.IsDeleted == false || u.IsDeleted == null)
                .Include(u => u.Role)
                .Include(u => u.Company)
                .Include(u => u.Department)
                .Include(u => u.Manager)
                .Include(u => u.Position)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Keyword))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(dto.Keyword) ||
                    u.LastName.Contains(dto.Keyword) ||
                    u.Email.Contains(dto.Keyword) ||
                    u.UserName.Contains(dto.Keyword));
            }

            if (dto.RoleId.HasValue)
                query = query.Where(u => u.RoleId == dto.RoleId);

            if (dto.CompanyId.HasValue)
                query = query.Where(u => u.CompanyId == dto.CompanyId);

            if (dto.DepartmentId.HasValue)
                query = query.Where(u => u.DepartmentId == dto.DepartmentId);

            if (dto.IsActive.HasValue)
                query = query.Where(u => u.IsActive == dto.IsActive.Value);

            var totalCount = await query.CountAsync();

            var users = await query
                .OrderByDescending(x => x.Id)
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync();

            return (users, totalCount);
        }
    }
}
