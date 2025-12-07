using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Services.IServices;
using System.Text;

namespace DMSAPI.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public UserService(IUserRepository userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<UserDTO>> GetAllUserAsync()
		{
			var users = await _userRepository.GetAllUserAsync();
			return _mapper.Map<IEnumerable<UserDTO>>(users);
		}

		public async Task<UserDTO> GetUserByEmailAsync(string email)
		{
			var user = await _userRepository.GetUserByEmailAsync(email);
			return _mapper.Map<UserDTO>(user);
		}

		public async Task<UserDTO> GetUserByIdAsync(int userId)
		{
			var user = await _userRepository.GetUserByIdsync(userId);
			return _mapper.Map<UserDTO>(user);
		}

		public async Task<UserDTO> UpdateUserAsync(UpdateUserDTO dto, int userId)
		{
			var user = await _userRepository.GetByIdAsync(dto.Id)
				?? throw new Exception("User not found");

			_mapper.Map(dto, user);
			user.UpdatedAt = DateTime.UtcNow;
			user.UpdatedBy = userId;

			await _userRepository.UpdateAsync(user);
			return _mapper.Map<UserDTO>(user);
		}

		public async Task<bool> PasswordResetAsync(PasswordResetDTO dto)
		{
			var user = await _userRepository.GetUserByEmailAsync(dto.Email);

			user.PasswordHash = Hash(dto.NewPassword, out string salt);
			user.PasswordSalt = salt;

			await _userRepository.UpdateAsync(user);
			return true;
		}

		public async Task<bool> PasswordUpdateAsync(PasswordUpdateDTO dto)
		{
			var user = await _userRepository.GetUserByEmailAsync(dto.Email);

			if (!Verify(dto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
				throw new Exception("Invalid password");

			user.PasswordHash = Hash(dto.NewPassword, out string salt);
			user.PasswordSalt = salt;

			await _userRepository.UpdateAsync(user);
			return true;
		}

		public async Task<bool> SetActiveStatusAsync(UserActiveStatusDTO dto)
		{
			var user = await _userRepository.GetByIdAsync(dto.Id);
			user.IsActive = dto.IsActive;

			await _userRepository.UpdateAsync(user);
			return true;
		}

		public async Task<IEnumerable<UserDTO>> GetEmployeesByManagerIdAsync(int managerId)
		{
			var users = await _userRepository.GetEmployeesByManagerIdAsync(managerId);
			return _mapper.Map<IEnumerable<UserDTO>>(users);
		}

		public async Task<UserDTO> SoftDeleteUser(int userId)
		{
			var user = await _userRepository.GetByIdAsync(userId);
			user.IsDeleted = true;
			user.IsActive = false;

			await _userRepository.UpdateAsync(user);
			return _mapper.Map<UserDTO>(user);
		}

		public async Task<PagedResultDTO<UserDTO>> SearchUsersAsync(UserSearchDTO dto)
		{
			var (users, total) = await _userRepository.SearchUsersAsync(dto);

			return new PagedResultDTO<UserDTO>
			{
				Items = _mapper.Map<List<UserDTO>>(users.ToList()),
				TotalCount = total,
				Page = dto.Page,
				PageSize = dto.PageSize
			};
		}

		private string Hash(string password, out string salt)
		{
			using var hmac = new System.Security.Cryptography.HMACSHA256();
			salt = Convert.ToBase64String(hmac.Key);
			return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
		}

		private bool Verify(string input, string hash, string salt)
		{
			using var hmac = new System.Security.Cryptography.HMACSHA256(Convert.FromBase64String(salt));
			return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(input))) == hash;
		}
	}
}
