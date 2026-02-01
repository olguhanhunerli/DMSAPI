using DMSAPI.Entities.DTOs.ComplaintDTO;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Presentation.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    public class ComplaintController : BaseApiController
    {
        private readonly IComplaintService _complaintService;
        private readonly IComplaintAttachmentService _complaintAttachmentService;
        public ComplaintController(IComplaintService complaintService, IComplaintAttachmentService complaintAttachmentService)
        {
            _complaintService = complaintService;
            _complaintAttachmentService = complaintAttachmentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllComplaints([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var complaints = await _complaintService.GetAllComplaintsAsync(pageNumber, pageSize);
            return Ok(complaints);
        }
        [HttpGet("{complaintNo}")]
        public async Task<IActionResult> GetComplaintById(string complaintNo)
        {
            var complaint = await _complaintService.GetComplaintByNoAsync(complaintNo);
            if (complaint == null)
            {
                return BadRequest();
            }
            return Ok(complaint);
        }
        [HttpPost]
        public async Task<IActionResult> CreateComplaint([FromBody] CreateComplaintDTO dto)
        {
            if (UserId == 0 || CompanyId == 0) return Unauthorized();
            dto.CompanyId = CompanyId;

            var complaint = await _complaintService.CreateComplaintAsync(dto, UserId, CompanyId);
            return Ok(complaint);
        }
        [HttpPost("close")]
        public async Task<IActionResult> UpdateIsClosed(string complaintNo)
        {
            await _complaintService.UpdateClosedAsync(complaintNo, UserId);
            return Ok();
        }
        [HttpDelete("{complaintNo}")]
        public async Task<IActionResult> DeleteComplaint(string complaintNo)
        {
            await _complaintService.DeleteComplaintAsync(complaintNo, UserId);
            return Ok();
        }
        [HttpPut("{complaintNo}")]
        public async Task<IActionResult> UpdateComplaint(string complaintNo, [FromBody] UpdateComplaintDTO dto)
        {
            if (UserId == 0 || CompanyId == 0) return Unauthorized();

            dto.CompanyId = CompanyId;
            var complaint = await _complaintService.UpdateComplaintByNoAsync(complaintNo, dto, UserId, CompanyId);
            return Ok(complaint);
        }
        [HttpGet("{complaintNo}/attachments")]
        public async Task<IActionResult> GetComplaintAttachments(string complaintNo)
        {
            var attachments = await _complaintAttachmentService.GetByComplaintNoAsync(complaintNo);
            return Ok(attachments);

        }
        [HttpPost("{complaintNo}/attachments")]
        public async Task<IActionResult> UploadComplaintAttachment(string complaintNo, IFormFile file)
        {
            if (UserId == 0) return Unauthorized();
            var attachment = await _complaintAttachmentService.UploadAsync(complaintNo, file, UserId);
            return Ok(attachment);
        }
		[HttpDelete("attachments/{id}")]
		public async Task<IActionResult> DeleteComplaintAttachment(long id)
		{
			if (UserId == 0) return Unauthorized();
			var result = await _complaintAttachmentService.DeleteAsync(id, UserId);
			if (!result)
				return NotFound();
			return Ok();
		}
        [HttpGet("attachments/download/{id}")]
		public async Task<IActionResult> DownloadComplaintAttachment(long id)
		{
			var (stream, contentType, downloadFileName) = await _complaintAttachmentService.DownloadAsync(id);
			return File(stream, contentType, downloadFileName);
		}
	}
}
