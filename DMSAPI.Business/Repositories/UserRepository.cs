using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DMSAPI.Business.Repositories
{
	public class UserRepository : GenericRepository<User>, IUserRepository
	{
		private readonly IHttpContextAccessor _http;

		public UserRepository(DMSDbContext context, IHttpContextAccessor accessor)
			: base(context, accessor)
		{
			_http = accessor;
		}

		private bool IsGlobalAdmin =>
			_http.HttpContext?.User?.FindFirst("role")?.Value == "GLOBAL ADMIN";

		public async Task<IEnumerable<User>> GetAllUserAsync()
		{
			var query = _dbSet
				.Where(u => u.IsDeleted == false || u.IsDeleted == null);

			if (!IsGlobalAdmin)
				query = query.Where(u => u.CompanyId == CompanyId);

			return await query
				.Include(u => u.Role)
				.Include(u => u.Company)
				.Include(u => u.Department)
				.Include(u => u.Manager)
				.Include(u => u.Position)
				.ToListAsync();
		}

		public async Task<IEnumerable<User>> GetEmployeesByManagerIdAsync(int managerId)
		{
			var query = _dbSet
				.Where(u =>
					u.ManagerId == managerId &&
					(u.IsDeleted == false || u.IsDeleted == null));

			if (!IsGlobalAdmin)
				query = query.Where(u => u.CompanyId == CompanyId);

			return await query
				.Include(u => u.Company)
				.Include(u => u.Department)
				.Include(u => u.Role)
				.ToListAsync();
		}

		public async Task<User?> GetUserByEmailAsync(string email)
		{
			return await _dbSet
				.Include(u => u.Role)
				.Include(u => u.Company)
				.Include(u => u.Department)
				.Include(u => u.Manager)
				.FirstOrDefaultAsync(u =>
					u.Email == email &&
					(u.IsDeleted == false || u.IsDeleted == null));
		}

		public async Task<User?> GetUserByEmailWithRoleAndPermissionsAsync(string email)
		{
			return await _dbSet
				.Include(u => u.Role)
				.Include(u => u.Company)
				.Include(u => u.Department)
				.Include(u => u.Manager)
				.Include(u => u.Position)
				.FirstOrDefaultAsync(u =>
					u.Email == email &&
					(u.IsDeleted == false || u.IsDeleted == null));
		}

		public async Task<User?> GetUserByIdsync(int id)
		{
			var query = _dbSet
				.Include(u => u.Role)
				.Include(u => u.Company)
				.Include(u => u.Department)
				.Include(u => u.Manager)
				.Include(u => u.Position)
				.Where(u => u.Id == id &&
							(u.IsDeleted == false || u.IsDeleted == null));

			if (!IsGlobalAdmin)
				query = query.Where(u => u.CompanyId == CompanyId);

			return await query.FirstOrDefaultAsync();
		}

		public async Task<User?> GetUserWithRelationsAsync(int id)
		{
			var query = _dbSet
				.Include(u => u.Role)
				.Include(u => u.Company)
				.Include(u => u.Department)
				.Include(u => u.Manager)
				.Where(u => u.Id == id &&
							(u.IsDeleted == false || u.IsDeleted == null));

			if (!IsGlobalAdmin)
				query = query.Where(u => u.CompanyId == CompanyId);

			return await query.FirstOrDefaultAsync();
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

			if (!IsGlobalAdmin)
				query = query.Where(u => u.CompanyId == CompanyId);

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
