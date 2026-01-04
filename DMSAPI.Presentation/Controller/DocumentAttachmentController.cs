using DMSAPI.Entities.DTOs.DocumentAttachmentDTO.cs;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentAttachmentController : BaseApiController
    {
        private readonly IDocumentAttachmentService _documentAttachmentService;

        public DocumentAttachmentController(IDocumentAttachmentService documentAttachmentService)
        {
            _documentAttachmentService = documentAttachmentService;
        }

        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultiple([FromForm] CreateDocumentAttachmentDTO dto)
        {
            await _documentAttachmentService.UploadMultipleAsync(dto, UserId);
            return Ok();
        }
		[HttpGet("download/{attachmentId}")]
		public async Task<IActionResult> Download(int attachmentId)
		{
			var result = await _documentAttachmentService.DownloadAttachmentAsync(attachmentId);

			return File(
				result.FileBytes,
				result.ContentType,
				result.DownloadFileName
			);
		}
	}
}
