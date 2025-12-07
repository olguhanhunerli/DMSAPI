using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Presentation.Controller;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/[controller]")]
public class UserController : BaseApiController
{
	private readonly IUserService _service;

	public UserController(IUserService service)
	{
		_service = service;
	}

	[HttpGet("get-all")]
	public async Task<IActionResult> GetAll()
		=> Ok(await _service.GetAllUserAsync());

	[HttpGet("get-by-id/{id}")]
	public async Task<IActionResult> GetById(int id)
		=> Ok(await _service.GetUserByIdAsync(id));

	[HttpGet("get-by-email")]
	public async Task<IActionResult> GetByEmail([FromQuery] string email)
		=> Ok(await _service.GetUserByEmailAsync(email));

	[HttpGet("get-by-manager/{managerId}")]
	public async Task<IActionResult> GetEmployees(int managerId)
		=> Ok(await _service.GetEmployeesByManagerIdAsync(managerId));

	[HttpPost("search")]
	public async Task<IActionResult> Search(UserSearchDTO dto)
		=> Ok(await _service.SearchUsersAsync(dto));

	[HttpPost("password-reset")]
	public async Task<IActionResult> PasswordReset(PasswordResetDTO dto)
		=> Ok(await _service.PasswordResetAsync(dto));

	[HttpPost("password-update")]
	public async Task<IActionResult> PasswordUpdate(PasswordUpdateDTO dto)
		=> Ok(await _service.PasswordUpdateAsync(dto));

	[HttpPost("active-status")]
	public async Task<IActionResult> SetActiveStatus(UserActiveStatusDTO dto)
		=> Ok(await _service.SetActiveStatusAsync(dto));

	[HttpPut("update")]
	public async Task<IActionResult> Update(UpdateUserDTO dto)
		=> Ok(await _service.UpdateUserAsync(dto, UserId));

	[HttpDelete("delete/{id}")]
	public async Task<IActionResult> SoftDelete(int id)
		=> Ok(await _service.SoftDeleteUser(id));
}
