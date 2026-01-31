using DMSAPI.Entities.DTOs.ComplaintDTO;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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
        public ComplaintController(IComplaintService complaintService)
        {
            _complaintService = complaintService;
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
        
	}
}
