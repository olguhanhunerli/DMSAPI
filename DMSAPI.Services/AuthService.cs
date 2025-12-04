using AutoMapper;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs;
using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IGenericRepository<RefreshToken> _refreshToken;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, ITokenService tokenService, IGenericRepository<RefreshToken> refreshToken, IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _refreshToken = refreshToken;
            _mapper = mapper;
        }

        public async Task<AuthResponseDTO> LoginAsync(UserLoginDTO loginDTO)
        {
            var user = await _userRepository.GetUserByEmailWithRoleAndPermissionsAsync(loginDTO.Email);
            if (user == null)
                throw new Exception("Invalid Credentials");

			if (user.IsDeleted == true)
				throw new Exception("User is Deleted");

			if (!user.IsActive)
                throw new Exception("User is deactivated");

			if (!VerifyPassword(loginDTO.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Invalid Credentials");
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            await _refreshToken.AddAsync(new RefreshToken
            {
                UserId = user.Id,
                TokenHash = _tokenService.HashToken(refreshToken.RawToken),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
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
            var hasnedToken = _tokenService.HashToken(refreshToken);
            var storedToken = (await _refreshToken.GetAllAsync())
                .FirstOrDefault(t => t.TokenHash == hasnedToken && t.ExpiresAt > DateTime.UtcNow);
            if (storedToken == null)
                throw new Exception("Invalid refresh token");
            var user = await _userRepository.GetByIdAsync(storedToken.UserId);
            var accessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            await _refreshToken.AddAsync(new RefreshToken
            {
                UserId = user.Id,
                TokenHash = _tokenService.HashToken(newRefreshToken.RawToken),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
            });
            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.RawToken,
                User = _mapper.Map<UserDTO>(user)
            };

        }

        public async Task<AuthResponseDTO> RegisterAsync(UserRegisterDTO registerDTO, int userIdFromToken)
        {
            var existing = await _userRepository.GetUserByEmailAsync(registerDTO.Email);
            if (existing != null)
                throw new Exception("User with this email already exists");

            using var hmac = new System.Security.Cryptography.HMACSHA256();
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
            var users = await _userRepository.GetByIdAsync(userIdFromToken)
                ?? throw new Exception("User not found");
            var user = new User
            {
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName,
                RoleId = registerDTO.RoleId,
                DepartmentId = registerDTO.DepartmentId,
                PositionId = registerDTO.PositionId,
                CompanyId = registerDTO.CompanyId,
                IsActive = true,
                CanApprove = registerDTO.CanApprove,
                ApprovalLevel = registerDTO.ApprovalLevel,
                ManagerId = registerDTO.ManagerId,
                Language = registerDTO.Language,
                TimeZone = registerDTO.TimeZone,
                Theme = registerDTO.Theme,
                NotificationPreferences = registerDTO.NotificationPreferences,
                PasswordHash = Convert.ToBase64String(hash),
                PasswordSalt = Convert.ToBase64String(salt),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userIdFromToken
                
            };

            await _userRepository.AddAsync(user);

            var createdUser = await _userRepository
                .GetUserByEmailWithRoleAndPermissionsAsync(user.Email);

            var accessToken = _tokenService.GenerateAccessToken(createdUser);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _refreshToken.AddAsync(new RefreshToken
            {
                UserId = createdUser.Id,
                TokenHash = _tokenService.HashToken(refreshToken.RawToken),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
            });

            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.RawToken,
                User = _mapper.Map<UserDTO>(createdUser)
            };

        }

        private bool VerifyPassword(string inputPassword, string storedHash, string storedSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256(Convert.FromBase64String(storedSalt));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputPassword)); 
            return Convert.ToBase64String(computedHash) == storedHash;

        }
    }
}
