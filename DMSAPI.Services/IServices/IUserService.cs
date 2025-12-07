using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.UserDTOs;

namespace DMSAPI.Services.IServices
{
	public interface IUserService
	{
		Task<IEnumerable<UserDTO>> GetAllUserAsync();

		Task<UserDTO> GetUserByEmailAsync(string email);

		Task<UserDTO> GetUserByIdAsync(int userId);

		Task<UserDTO> UpdateUserAsync(
			UpdateUserDTO updateUserDTO,
			int userIdFromToken
		);

		Task<bool> PasswordResetAsync(PasswordResetDTO passwordResetDTO);

		Task<bool> PasswordUpdateAsync(PasswordUpdateDTO passwordUpdateDTO);

		Task<bool> SetActiveStatusAsync(UserActiveStatusDTO userActiveStatusDTO);

		Task<IEnumerable<UserDTO>> GetEmployeesByManagerIdAsync(int managerId);

		Task<UserDTO> SoftDeleteUser(int userId);

		Task<PagedResultDTO<UserDTO>> SearchUsersAsync(UserSearchDTO dto);
	}
}
