using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Presentation.Controller;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/[controller]")]
public class DepartmentController : BaseApiController
{
	private readonly IDepartmentService _service;

	public DepartmentController(IDepartmentService service)
	{
		_service = service;
	}

	[HttpGet("get-all")]
	public async Task<IActionResult> GetAll()
		=> Ok(await _service.GetAllDepartmentsAsync());

	[HttpGet("get-by-id/{id}")]
	public async Task<IActionResult> GetById(int id)
		=> Ok(await _service.GetDepartmentByIdAsync(id));

	[HttpPost("create")]
	public async Task<IActionResult> Create(CreateDepartmentDTO dto)
		=> Ok(await _service.CreateDepartmentAsync(dto, UserId));

	[HttpPut("update")]
	public async Task<IActionResult> Update(UpdateDepartmentDTO dto)
		=> Ok(await _service.UpdateDepartmentAsync(dto, UserId));
	[Authorize(Roles = "GLOBAL ADMIN,SUPER ADMIN")]
	[HttpDelete("delete/{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		await _service.DeleteDepartmentAsync(id, UserId);
		return Ok();
	}
}
