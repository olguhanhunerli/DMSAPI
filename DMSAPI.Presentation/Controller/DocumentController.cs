using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Presentation.Controller;
using DMSAPI.Services;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/[controller]")]
public class DocumentController : BaseApiController
{
	private readonly IDocumentService _service;

	public DocumentController(IDocumentService service)
	{
		_service = service;
	}

	[HttpPost("create")]
	public async Task<IActionResult> Create([FromForm] DocumentCreateDTO dto)
		=> Ok(await _service.CreateDocumentAsync(dto, UserId));

	[HttpGet("get-all")]
	public async Task<IActionResult> GetAll()
		=> Ok(await _service.GetAllDocumentsAsync());
    [HttpGet("get-paged")]
    public async Task<IActionResult> GetPaged(
   int page = 1,
   int pageSize = 10)
    {
        var userId = UserId;             
        var roleId = RoleId;
        var departmentId = DepartmentId;

        var result = await _service
            .GetPageAsync(page, pageSize, userId, roleId, departmentId);

        return Ok(result);
    }
	[HttpGet("create-preview")]
	public async Task<IActionResult> CreatePreview(int categoryId)
	{
		var result = await _service.GetCreatePreviewAsync(categoryId, UserId);

		return Ok(result);
	}
    [HttpGet("my-pending-approvals")]
    public async Task<IActionResult> GetMyPendingApprovals()
    {
        int userId = int.Parse(User.FindFirst("sub").Value);

        var docs = await _service.GetMyPendingApprovalsAsync(userId);

        return Ok(docs);
    }
    [HttpGet("my-pending-approvals/paged")]
    public async Task<IActionResult> GetMyPendingApprovals(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
    {
        int userId = int.Parse(User.FindFirst("sub")!.Value);

        var result = await _service
            .GetMyPendingApprovalsAsync(page, pageSize, userId);

        return Ok(result);
    }


}
