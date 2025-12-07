using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Entities.Models;

namespace DMSAPI.Business.Repositories.IRepositories
{
	public interface IUserRepository : IGenericRepository<User>
	{
		Task<IEnumerable<User>> GetAllUserAsync();

		Task<User?> GetUserByEmailAsync(string email);
		Task<User?> GetUserByEmailWithRoleAndPermissionsAsync(string email);

		Task<User?> GetUserWithRelationsAsync(int id);
		Task<User?> GetUserByIdsync(int id);

		Task<IEnumerable<User>> GetEmployeesByManagerIdAsync(int managerId);

		Task<(IEnumerable<User> Users, int TotalCount)> SearchUsersAsync(UserSearchDTO dto);
	}
}
