using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Presentation.Controller;
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
        var result = await _service.GetPageAsync(page, pageSize);
        return Ok(result);
    }


}
