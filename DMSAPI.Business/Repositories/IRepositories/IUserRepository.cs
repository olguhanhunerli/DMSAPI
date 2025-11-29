using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs;
using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User> GetUserByEmailWithRoleAndPermissionsAsync(string email);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserWithRelationsAsync(int id);
        Task<IEnumerable<User>> GetEmployeesByManagerIdAsync(int managerId);
		Task<User> GetUserByIdsync(int id);
        Task<IEnumerable<User>> SearchUsersAsync(UserSearchDTO userSearchDTO);
	}
}
