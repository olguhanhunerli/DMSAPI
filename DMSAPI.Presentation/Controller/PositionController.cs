using DMSAPI.Entities.DTOs.PositionDTOs;
using DMSAPI.Presentation.Controller;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/[controller]")]
public class PositionController : BaseApiController
{
	private readonly IPositionService _service;

	public PositionController(IPositionService service)
	{
		_service = service;
	}

	[HttpGet("get-all")]
	public async Task<IActionResult> GetAll()
		=> Ok(await _service.GetAllPositionsAsync());
	[Authorize(Roles = "GLOBAL ADMIN,SUPER ADMIN")]
	[HttpGet("get-by-id/{id}")]
	public async Task<IActionResult> GetById(int id)
		=> Ok(await _service.GetPositionByIdAsync(id));
	[Authorize(Roles = "GLOBAL ADMIN,SUPER ADMIN")]
	[HttpPost("create")]
	public async Task<IActionResult> Create(CreatePositionDTO dto)
	{
		await _service.AddPositionAsync(dto, UserId);
		return Ok();
	}
	[HttpPut("update")]
	public async Task<IActionResult> Update(UpdatePositionDTO dto)
	{
		var response = await _service.UpdatePositionAsync(dto, UserId);
		return Ok(response);
	}
    [Authorize(Roles = "GLOBAL ADMIN,SUPER ADMIN")]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeletePositionAsync(id, UserId);
        return Ok();
    }
}
