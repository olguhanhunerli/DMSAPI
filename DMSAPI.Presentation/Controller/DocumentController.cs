using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.DTOs.Revision;
using DMSAPI.Entities.Models;
using DMSAPI.Presentation.Controller;
using DMSAPI.Services;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

[ApiController]
[Route("api/[controller]")]
public class DocumentController : BaseApiController
{
	private readonly IDocumentService _service;
	private readonly IDocumentAccessLogService _documentAccessLogService;
	private readonly IDocumentRevisionService _revisionService;

	public DocumentController(IDocumentService service, IDocumentAccessLogService documentAccessLogService, IDocumentRevisionService revisionService)
	{
		_service = service;
		_documentAccessLogService = documentAccessLogService;
		_revisionService = revisionService;
	}

	[HttpPost("create")]
	[Consumes("multipart/form-data")]
	public async Task<IActionResult> Create([FromForm] DocumentCreateDTO dto)
	{
		Console.WriteLine("🔥 DOCUMENT CREATE API HIT 🔥");

		if (!ModelState.IsValid)
		{
			Console.WriteLine("❌ ModelState Invalid");
			return BadRequest(ModelState);
		}

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
		await _documentAccessLogService.AddAsync(new DocumentAccessLog
		{
			DocumentId = id,
			UserId = UserId,
			AccessType = "ORIGINAL DOCUMENT VIEW",
			AccessAt = DateTime.UtcNow,
			IpAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()
		});
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
		await _documentAccessLogService.AddAsync(new DocumentAccessLog
		{
			DocumentId = id,
			UserId = UserId,
			AccessType = "PDF DOCUMENT VIEW",
			AccessAt = DateTime.UtcNow,
			IpAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()
		});
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
		try
		{
			var result = await _service.GetCreatePreviewAsync(categoryId, UserId);
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
		await _documentAccessLogService.AddAsync(new DocumentAccessLog
		{
			DocumentId = documentId,
			UserId = UserId,
			AccessType = "Original Document Download",
			AccessAt = DateTime.UtcNow,
			IpAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()
		});
		return File(
			result.FileBytes,
			result.ContentType,
			result.DownloadFileName

		);
	}
	[AllowAnonymous]
	[HttpGet("download-pdf/{documentId}")]
	public async Task<IActionResult> DownloadPdf(int documentId)
	{
		var result = await _service.DownloadPdfAsync(documentId);
		await _documentAccessLogService.AddAsync(new DocumentAccessLog
		{
			DocumentId = documentId,
			UserId = UserId,
			AccessType = "PDF Document Download",
			AccessAt = DateTime.UtcNow,
			IpAddress = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()
		});
		return File(
			result.FileBytes,
			"application/pdf",
			result.DownloadFileName

		);
	}
	[AllowAnonymous]
	[HttpGet("get-paged-by-category")]
	public async Task<IActionResult> GetPagedByCategory(
		int categoryId,
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 10)
	{
		var userId = UserId;
		var roleId = RoleId;
		var departmentId = DepartmentId;
		var result = await _service
			.GetPagedByCategoryAsync(page, pageSize, categoryId, userId, roleId, departmentId);
		return Ok(result);
	}
	[HttpPost("{documentId}/start-revision")]
	public async Task<IActionResult> StartRevision(int documentId, [FromBody] StartRevisionDTO dto)
	{
		await _revisionService.StartRevisionAsync(documentId, UserId, dto.RevisionNote);
		return Ok(new { message = "Revizyon Başlatıldı." });
	}
	[HttpPost("{documentId}/cancel-revision")]
	public async Task<IActionResult> CancelRevision(int documentId, [FromBody] CancelRevisionDTO dto)
	{
		await _revisionService.CancelRevisiyonAsync(documentId, UserId, dto.Reason);
		return Ok(new { message = "Revizyon İptal Edildi." });
	}
	[HttpPost("{documentId}/finish-reservation")]
	[Consumes("multipart/form-data")]
	public async Task<IActionResult> FinishReservation(int documentId, [FromBody] FinishRevisionDTO dto)
	{
		var folder = Path.Combine(

			Directory.GetCurrentDirectory(),
			"wwwroot",
			"uploads",
			"documents",
			documentId.ToString(),
			"revisions"
		);
		Directory.CreateDirectory(folder);
		var cleanName = Path.GetFileName(dto.MainFile.FileName);
		var fullPath = Path.Combine(folder, cleanName);
		using (var stream = new FileStream(fullPath, FileMode.Create))
		{
			await dto.MainFile.CopyToAsync(stream);
		}

		await _revisionService.FinishReservationAsync(
			documentId, UserId, fullPath, dto.Approval);
		return Ok(new { message = "Revizyon Tamamlandı." });
	}
	[HttpGet("{documentId}/revision-preview")]
	public async Task<IActionResult> GetRevisionPreview(int documentId)
	{
		try
		{
			var result = await _service.GetRevisionPreviewAsync(documentId, UserId);
			return Ok(result);
		}
		catch (Exception ex)
		{
			return StatusCode(409, new
			{
				message = ex.Message,
				detail = ex.InnerException?.Message
			});
		}
	}
	[HttpGet("my-active-revision")]
	public async Task<IActionResult> GetMyActiveRevision(int page=1, int pageSize=10)
	{
		var result = await _revisionService.GetMyActiveRevisionAsync(UserId, page, pageSize);
		return Ok(result);
	}

}
