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
    [Authorize(Roles = "ADMIN,SUPER ADMIN")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController: BaseApiController
    {
        private readonly IDocumentAccessLogService _documentAccessLogService;

        public AuditController(IDocumentAccessLogService documentAccessLogService)
        {
            _documentAccessLogService = documentAccessLogService;
        }
        [HttpGet("document-access-logs")]
        public async Task<IActionResult> GetDocumentAccessLogs([FromQuery] DocumentAccessLogFilterDTO dTO)
        {
            var logs = await _documentAccessLogService.GetAllAsync(dTO);
            return Ok(logs);
        }
    }
}
