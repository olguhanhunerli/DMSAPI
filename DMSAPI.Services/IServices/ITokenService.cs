using DMSAPI.Entities.DTOs;
using DMSAPI.Entities.Models;

namespace DMSAPI.Services.IServices
{
	public interface ITokenService
	{
		string GenerateAccessToken(User user);
		RefreshTokenDTO GenerateRefreshToken();
		string HashToken(string token);
	}
}
