using DMSAPI.Entities.DTOs.RoleDTOs;
using DMSAPI.Presentation.Authorization;
using DMSAPI.Presentation.Controller;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Authorize(Roles = RoleGroups.MasterDataWrite)]
[Route("api/[controller]")]
public class RoleController : BaseApiController
{
    private readonly IRoleService _service;

    public RoleController(IRoleService service)
    {
        _service = service;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllRoleAsync());

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _service.GetByIdAsync(id));
    [HttpGet("get-paged")]
    public async Task<IActionResult> GetPaged(
    int page = 1,
    int pageSize = 10)
    {
        var result = await _service.GetPagedAsync(page, pageSize);
        return Ok(result);
    }
    [HttpPost("create")]
    public async Task<IActionResult> Create(AddRoleDTO dto)
    {
        await _service.AddAsync(dto, UserId);
        return Ok();
    }
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, UpdateRoleDTO dto)
    {
        await _service.UpdateAsync(id, dto, UserId);
        return Ok();
    }
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id, UserId);
        return Ok();
    }
}

