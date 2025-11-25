using AutoMapper;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs;
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

            if(user == null || !VerifyPassword(loginDTO.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Invalid Credentials");
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

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
        private bool VerifyPassword(string inputPassword, string storedHash, string storedSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(storedSalt));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputPassword)); 
            return Convert.ToBase64String(computedHash) == storedHash;

        }
    }
}
