using AutoMapper;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs;
using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using System.Security.Cryptography;
using System.Text;

namespace DMSAPI.Services
{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository _userRepository;
		private readonly ITokenService _tokenService;
		private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
		private readonly IMapper _mapper;

		public AuthService(
			IUserRepository userRepository,
			ITokenService tokenService,
			IGenericRepository<RefreshToken> refreshTokenRepository,
			IMapper mapper)
		{
			_userRepository = userRepository;
			_tokenService = tokenService;
			_refreshTokenRepository = refreshTokenRepository;
			_mapper = mapper;
		}

		public async Task<AuthResponseDTO> LoginAsync(UserLoginDTO loginDTO)
		{
			var user = await _userRepository.GetUserByEmailWithRoleAndPermissionsAsync(loginDTO.Email)
				?? throw new Exception("Invalid credentials");

			if (user.IsDeleted == true)
				throw new Exception("User is deleted");

			if (!user.IsActive)
				throw new Exception("User is passive");

			if (!VerifyPassword(loginDTO.Password, user.PasswordHash, user.PasswordSalt))
				throw new Exception("Invalid credentials");

			user.LastLoginAt = DateTime.UtcNow;
			await _userRepository.UpdateAsync(user);

			var accessToken = _tokenService.GenerateAccessToken(user);
			var refreshToken = _tokenService.GenerateRefreshToken();

			await _refreshTokenRepository.AddAsync(new RefreshToken
			{
				UserId = user.Id,
				TokenHash = _tokenService.HashToken(refreshToken.RawToken),
				ExpiresAt = DateTime.UtcNow.AddDays(7)
			});

			return new AuthResponseDTO
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken.RawToken,
				User = _mapper.Map<UserDTO>(user)
			};
		}

		public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken)
		{
			var hash = _tokenService.HashToken(refreshToken);

			var storedToken = (await _refreshTokenRepository.GetAllAsync())
				.FirstOrDefault(x => x.TokenHash == hash && x.ExpiresAt > DateTime.UtcNow)
				?? throw new Exception("Invalid refresh token");

			var user = await _userRepository.GetByIdAsync(storedToken.UserId)
				?? throw new Exception("User not found");

			var newAccess = _tokenService.GenerateAccessToken(user);
			var newRefresh = _tokenService.GenerateRefreshToken();

			await _refreshTokenRepository.AddAsync(new RefreshToken
			{
				UserId = user.Id,
				TokenHash = _tokenService.HashToken(newRefresh.RawToken),
				ExpiresAt = DateTime.UtcNow.AddDays(7)
			});

			return new AuthResponseDTO
			{
				AccessToken = newAccess,
				RefreshToken = newRefresh.RawToken,
				User = _mapper.Map<UserDTO>(user)
			};
		}

		public async Task<AuthResponseDTO> RegisterAsync(UserRegisterDTO dto, int userIdFromToken)
		{
			var existing = await _userRepository.GetUserByEmailAsync(dto.Email);
			if (existing != null)
				throw new Exception("User already exists");

			var hash = Hash(dto.Password, out string salt);

			var user = new User
			{
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Email = dto.Email,
				PhoneNumber = dto.PhoneNumber,
				UserName = dto.UserName,
				RoleId = dto.RoleId,
				DepartmentId = dto.DepartmentId,
				PositionId = dto.PositionId,
				ManagerId = dto.ManagerId,
				CompanyId = dto.CompanyId,

				CanApprove = dto.CanApprove,
				ApprovalLevel = dto.ApprovalLevel,

				Language = dto.Language,
				TimeZone = dto.TimeZone,
				Theme = dto.Theme,
				NotificationPreferences = dto.NotificationPreferences,

				PasswordHash = hash,
				PasswordSalt = salt,

				IsActive = true,
				IsDeleted = false,
				CreatedAt = DateTime.UtcNow,
				CreatedBy = userIdFromToken
			};

			await _userRepository.AddAsync(user);

			var createdUser = await _userRepository.GetUserByEmailWithRoleAndPermissionsAsync(user.Email);

			var accessToken = _tokenService.GenerateAccessToken(createdUser);
			var refreshToken = _tokenService.GenerateRefreshToken();

			await _refreshTokenRepository.AddAsync(new RefreshToken
			{
				UserId = createdUser.Id,
				TokenHash = _tokenService.HashToken(refreshToken.RawToken),
				ExpiresAt = DateTime.UtcNow.AddDays(7)
			});

			return new AuthResponseDTO
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken.RawToken,
				User = _mapper.Map<UserDTO>(createdUser)
			};
		}

		private string Hash(string password, out string salt)
		{
			using var hmac = new HMACSHA256();
			salt = Convert.ToBase64String(hmac.Key);
			return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
		}

		private bool VerifyPassword(string input, string hash, string salt)
		{
			using var hmac = new HMACSHA256(Convert.FromBase64String(salt));
			var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
			return Convert.ToBase64String(computed) == hash;
		}
	}
}
