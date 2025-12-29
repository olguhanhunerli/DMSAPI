using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Presentation.Controller;
using DMSAPI.Services;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
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
        Console.WriteLine("=== API CREATE ===");

        if (dto.AllowedDepartmentIds == null)
            Console.WriteLine("AllowedDepartmentIds = NULL");
        else
            Console.WriteLine("AllowedDepartmentIds = " +
                string.Join(",", dto.AllowedDepartmentIds));

        Console.WriteLine("Form Keys: " +
            string.Join(",", Request.Form.Keys));
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
    int roleId, int departmentId,
    int page = 1,
    int pageSize = 10)
    {
        var result = await _service.GetPagedApprovedAsync(page, pageSize, UserId, roleId, departmentId);
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
			enableRangeProcessing: true
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
        

        var docs = await _service.GetMyPendingApprovalsAsync(UserId);

        return Ok(docs);
    }
    [HttpGet("my-pending-approvals/paged")]
    public async Task<IActionResult> GetMyPendingApprovals(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
    {
        

        var result = await _service
            .GetMyPendingApprovalsAsync(page, pageSize, UserId);

        return Ok(result);
    }
	[HttpGet("rejected")]
	public async Task<IActionResult> GetRejectedDocuments(
		   [FromQuery] int page = 1,
		   [FromQuery] int pageSize = 10)
	{
		var result = await _service
			.GetRejectedDocumentsAsync(page, pageSize);

		return Ok(result);
	}
	[AllowAnonymous]
	[HttpGet("download/{documentId}")]
	public async Task<IActionResult> Download(int documentId)
	{
		var result = await _service.DownloadDocumentFileAsync(documentId);

		return File(
			result.FileBytes,
			result.ContentType,
			result.OriginalFileName
		);
	}
	[AllowAnonymous]
	[HttpGet("download-pdf/{documentId}")]
	public async Task<IActionResult> DownloadPdf(int documentId)
	{
		var result = await _service.DownloadPdfAsync(documentId);

		return File(
			result.FileBytes,
			"application/pdf",
			result.OriginalFileName
		);
	}

}
