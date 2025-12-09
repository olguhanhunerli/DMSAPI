using DMSAPI.Entities.DTOs.CategoryDTOs;
using DMSAPI.Presentation.Controller;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/[controller]")]
public class CategoryController : BaseApiController
{
	private readonly ICategoryServices _service;

	public CategoryController(ICategoryServices service)
	{
		_service = service;
	}

	[HttpGet("get-all")]
	public async Task<IActionResult> GetAll()
		=> Ok(await _service.GetAllCategoriesAsync());

	[HttpGet("get-by-id/{id}")]
	public async Task<IActionResult> GetById(int id)
		=> Ok(await _service.GetCategoryByIdAsync(id));

	[HttpGet("get-tree")]
	public async Task<IActionResult> GetTree()
		=> Ok(await _service.GetCategoryTreeAsync(CompanyId));

    [HttpGet("breadcrumb/{id}")]
    public async Task<IActionResult> Breadcrumb(int id)
        => Ok(await _service.GetCategoryBreadcrumbDetailedAsync(id));
    [HttpGet("get-paged")]
    public async Task<IActionResult> GetPaged(
    int page = 1,
    int pageSize = 10)
    {
        var result = await _service.GetPagedAsync(page, pageSize);
        return Ok(result);
    }
    [HttpPost("search")]
	public async Task<IActionResult> Search(CategorySearchDTO dto)
		=> Ok(await _service.SearchCategoryTreeAsync(dto.Keyword, CompanyId));

	[HttpPost("create")]
	public async Task<IActionResult> Create(CreateCategoryDTO dto)
	{
		dto.CompanyId = CompanyId;
		return Ok(await _service.CreateCategoryAsync(dto, UserId));
	}

	[HttpPut("update")]
	public async Task<IActionResult> Update(UpdateCategoryDTO dto)
		=> Ok(await _service.UpdateCategoryByIdAsync(dto, UserId));

	[HttpDelete("delete")]
	public async Task<IActionResult> SoftDelete([FromBody] CategoryDeleteDTO dto)
		=> Ok(await _service.SoftDeleteCategoryAsync(dto, UserId));

	[HttpPut("restore")]
	public async Task<IActionResult> Restore(CategoryRestoreDTO dto)
		=> Ok(await _service.RestoreCategoryAsync(dto, UserId));

}
