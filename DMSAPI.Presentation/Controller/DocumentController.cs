using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Presentation.Controller
{
	[ApiController]
	[Route("api/[controller]")]
	public class DocumentController : ControllerBase
	{
		private readonly IDocumentService _documentService;

		public DocumentController(IDocumentService documentService)
		{
			_documentService = documentService;
		}
		[HttpPost("CreateDocument")]
		public async Task<IActionResult> CreateDocument([FromForm] DocumentCreateDTO createDocumentDTO)
		{
			var userId = GetUserId();
			var documentId = await _documentService.CreateDocumentAsync(createDocumentDTO, userId);
			return Ok(documentId);
		}
		[HttpGet("GetAllDocuments")]
		public async Task<IActionResult> GetAllDocuments()
		{
			var documents = await _documentService.GetAllDocumentsAsync();
			return Ok(documents);
		}
        [Authorize]
        [HttpGet("debug")]
        public IActionResult Debug()
        {
            return Ok(User.Claims.Select(x => new { x.Type, x.Value }));
        }

        private int GetUserId()
		{
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userId == null)
				throw new Exception("User ID not found in token");

			return int.Parse(userId);
		}
	}
}
