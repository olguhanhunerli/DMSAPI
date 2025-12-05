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

            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            return userDtos;
        }

		public async Task<IEnumerable<UserDTO>> GetEmployeesByManagerIdAsync(int managerId)
		{
			var users = await _userRepository.GetEmployeesByManagerIdAsync(managerId);
			var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
			return userDtos;
		}

		public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            var userDto = _mapper.Map<UserDTO>(user);
            return userDto;
        }

		public async Task<UserDTO> GetUserByIdAsync(int userId)
		{
			var user = await _userRepository.GetUserByIdsync(userId);
			var userDto = _mapper.Map<UserDTO>(user);
			return userDto;
		}

		public async Task<bool> PasswordResetAsync(PasswordResetDTO dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("User not found");

            var hashed = HashPasswordHmac(dto.NewPassword, out string salt);

            user.PasswordHash = hashed;
            user.PasswordSalt = salt;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> PasswordUpdateAsync(PasswordUpdateDTO dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("User not found");

            if (!VerifyPasswordHmac(dto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Current password is incorrect");

            var newHash = HashPasswordHmac(dto.NewPassword, out string newSalt);

            user.PasswordHash = newHash;
            user.PasswordSalt = newSalt;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<PagedResultDTO<UserDTO>> SearchUsersAsync(UserSearchDTO dto)
        {
            var (users, totalCount) = await _userRepository.SearchUsersAsync(dto);

            var userDtos = _mapper.Map<List<UserDTO>>(users);

            return new PagedResultDTO<UserDTO>
            {
                Items = userDtos,
                TotalCount = totalCount,
                Page = dto.Page,
                PageSize = dto.PageSize
            };
        }

        public async Task<bool> SetActiveStatusAsync(UserActiveStatusDTO userActiveStatusDTO)
        {
            var user = await _userRepository.GetByIdAsync(userActiveStatusDTO.Id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.IsActive = userActiveStatusDTO.IsActive;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            return true;
        }

		public async Task<UserDTO> SoftDeleteUser(int userId)
		{
			var user = await _userRepository.GetByIdAsync(userId);
			if (user == null)
			{
				throw new Exception("User not found");
			}
            user.IsDeleted = true;
            user.IsActive = false;
			user.UpdatedAt = DateTime.UtcNow;
			await _userRepository.UpdateAsync(user);
			return _mapper.Map<UserDTO>(user);

		}

		public async Task<UserDTO> UpdateUserAsync(UpdateUserDTO updateUserDTO, int userIdFromToken)
        {
            var user = await _userRepository.GetByIdAsync(updateUserDTO.Id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (updateUserDTO.FirstName != null) user.FirstName = updateUserDTO.FirstName;
            if (updateUserDTO.LastName != null) user.LastName = updateUserDTO.LastName;
            if (updateUserDTO.Email != null && updateUserDTO.Email != user.Email)
            {
                var emailExists = (await _userRepository.FindAsync(u => u.Email == updateUserDTO.Email))
                                   .Any();

                if (emailExists)
                    throw new Exception("Email is already in use");

                user.Email = updateUserDTO.Email;
            }
            if (updateUserDTO.PhoneNumber != null) user.PhoneNumber = updateUserDTO.PhoneNumber;
            if (updateUserDTO.UserName != null) user.UserName = updateUserDTO.UserName;
            if (updateUserDTO.RoleId.HasValue) user.RoleId = updateUserDTO.RoleId.Value;
            if (updateUserDTO.DepartmentId.HasValue) user.DepartmentId = updateUserDTO.DepartmentId.Value;
            if (updateUserDTO.CompanyId.HasValue) user.CompanyId = updateUserDTO.CompanyId.Value;
            if (updateUserDTO.ManagerId.HasValue) user.ManagerId = updateUserDTO.ManagerId.Value;
            if (updateUserDTO.PositionId.HasValue)
                user.PositionId = updateUserDTO.PositionId.Value;
            if (updateUserDTO.CanApprove.HasValue) user.CanApprove = updateUserDTO.CanApprove.Value;
            if (updateUserDTO.ApprovalLevel.HasValue) user.ApprovalLevel = updateUserDTO.ApprovalLevel.Value;
            if (updateUserDTO.IsActive.HasValue) user.IsActive = updateUserDTO.IsActive.Value;
            if (updateUserDTO.IsLocked.HasValue) user.IsLocked = updateUserDTO.IsLocked.Value;
            if (updateUserDTO.Language != null) user.Language = updateUserDTO.Language;
            if (updateUserDTO.TimeZone != null) user.TimeZone = updateUserDTO.TimeZone;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = userIdFromToken;
            await _userRepository.UpdateAsync(user);
            var updatedUser = await _userRepository.GetUserWithRelationsAsync(updateUserDTO.Id);
            return _mapper.Map<UserDTO>(updatedUser);
        }
        
        private string HashPasswordHmac(string password, out string salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256();
            salt = Convert.ToBase64String(hmac.Key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
        private bool VerifyPasswordHmac(string inputPassword, string storedHash, string storedSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256(Convert.FromBase64String(storedSalt));
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));
            return Convert.ToBase64String(computed) == storedHash;
        }

    }

}


