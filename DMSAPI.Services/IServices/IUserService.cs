using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUserAsync();
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> UpdateUserAsync(UpdateUserDTO updateUserDTO, int userIdFromToken);
        Task<bool> PasswordResetAsync(PasswordResetDTO passwordResetDTO);
        Task<bool> PasswordUpdateAsync(PasswordUpdateDTO passwordUpdateDTO);
        Task<bool> SetActiveStatusAsync(UserActiveStatusDTO userActiveStatusDTO);
		Task<IEnumerable<UserDTO>> GetEmployeesByManagerIdAsync(int managerId);
        Task<UserDTO> SoftDeleteUser(int userId);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserDTO>> SearchUsersAsync(UserSearchDTO userSearchDTO);
	}
}
