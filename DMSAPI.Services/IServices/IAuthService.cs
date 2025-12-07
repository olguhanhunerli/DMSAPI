using DMSAPI.Entities.DTOs;
using DMSAPI.Entities.DTOs.UserDTOs;

namespace DMSAPI.Services.IServices
{
	public interface IAuthService
	{
		Task<AuthResponseDTO> LoginAsync(UserLoginDTO loginDTO);

		Task<AuthResponseDTO> RegisterAsync(
			UserRegisterDTO registerDTO,
			int userIdFromToken
		);

		Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken);
	}
}
