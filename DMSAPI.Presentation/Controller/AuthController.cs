using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Presentation.Controller;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class AuthController : BaseApiController
{
	private readonly IAuthService _service;

	public AuthController(IAuthService service)
	{
		_service = service;
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<IActionResult> Login(UserLoginDTO dto)
		=> Ok(await _service.LoginAsync(dto));

	[AllowAnonymous]
	[HttpPost("register")]
	public async Task<IActionResult> Register(UserRegisterDTO dto)

		=> Ok(await _service.RegisterAsync(dto, UserId));

	[HttpPost("refresh-token")]
	[AllowAnonymous]
	public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
		=> Ok(await _service.RefreshTokenAsync(refreshToken));
	[HttpGet("debug-token")]
	public IActionResult Debug()
		=> Ok(User.Claims.Select(x => new { x.Type, x.Value }));
}
