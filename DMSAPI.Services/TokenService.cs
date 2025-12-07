using DMSAPI.Entities.DTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DMSAPI.Services
{
	public class TokenService : ITokenService
	{
		private readonly JwtSettings _jwtSettings;

		public TokenService(IOptions<JwtSettings> options)
		{
			_jwtSettings = options.Value;
		}

		public string GenerateAccessToken(User user)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim("role", user.Role?.Name ?? string.Empty),
				new Claim("userName", user.UserName ?? string.Empty),
				new Claim("companyId", user.CompanyId.ToString())
			};

			if (user.PositionId.HasValue)
			{
				claims.Add(new Claim("positionId", user.PositionId.Value.ToString()));
				claims.Add(new Claim("positionName", user.Position?.Name ?? string.Empty));
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public RefreshTokenDTO GenerateRefreshToken()
		{
			return new RefreshTokenDTO
			{
				RawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
			};
		}

		public string HashToken(string token)
		{
			using var sha = SHA256.Create();
			var bytes = Encoding.UTF8.GetBytes(token);
			var hash = sha.ComputeHash(bytes);
			return Convert.ToBase64String(hash);
		}
	}
}
