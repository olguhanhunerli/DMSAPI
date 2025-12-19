using DMSAPI.Entities.DTOs.DocumentDTOs;
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

    [ApiController]
    [Route("api/[controller]")]
    public class DocumentApprovalController: BaseApiController
    {
        private readonly IDocumentApprovalService _service;

        public DocumentApprovalController(IDocumentApprovalService service)
        {
            _service = service;
        }
        [HttpPost("init-approval")]
        public async Task<IActionResult> InitApproval([FromBody] CreateDocumentApprovalDTO dto)
        {
            await _service.CreateApprovalFlowAsync(dto, UserId);
            return Ok();
        }

        [HttpPost("approve")]
        public async Task<IActionResult> Approve(int documentId)
        {
            await _service.ApproveAsync(documentId, UserId);
            return Ok();
        }

        [HttpPost("reject")]
        public async Task<IActionResult> Reject(int documentId, string reason)
        {
            await _service.RejectAsync(documentId, UserId, reason);
            return Ok();
        }
    }
}
