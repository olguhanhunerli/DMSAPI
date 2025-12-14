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
    {
        try
        {
            if (Request.Form.Files.Any(f => f.Name == "MainFile"))
                dto.MainFile = Request.Form.Files["MainFile"];

            dto.Attachments = Request.Form.Files
                .Where(f => f.Name == "Attachments")
                .ToList();

            var result = await _service.CreateDocumentAsync(dto, UserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = ex.Message,
                detail = ex.InnerException?.Message
            });
        }
    }
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
    [HttpGet("approved")]
    public async Task<IActionResult> GetApproved(
    int page = 1,
    int pageSize = 10)
    {
        var result = await _service.GetPagedApprovedAsync(page, pageSize);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocumentDetailById(int id)
    {
        var document = await _service.GetDetailByIdAsync(id);
		if (document == null)
		{
			return NotFound(new { message = "Document not found." });
		}
		return Ok(document);
	}
	[HttpGet("{id}/pdf")]
	public async Task<IActionResult> GetDocumentPdf(int id)
	{
		var document = await _service.GetDetailByIdAsync(id);

		if (document == null || document.MainFile == null)
			return NotFound();

		if (string.IsNullOrWhiteSpace(document.MainFile.PdfFilePath))
			return NotFound("PDF not found");

		var fullPath = Path.Combine(
			Directory.GetCurrentDirectory(),
			document.MainFile.PdfFilePath.Replace("/", Path.DirectorySeparatorChar.ToString())
		);

		if (!System.IO.File.Exists(fullPath))
			return NotFound("PDF file missing on disk");

		return PhysicalFile(
			fullPath,
			"application/pdf",
			enableRangeProcessing: true // pdf.js için önemli
		);
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
