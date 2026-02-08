using DMSAPI.Entities.DTOs.CapaActionFileDTO;
using DMSAPI.Entities.DTOs.CAPADTO;
using DMSAPI.Presentation.Authorization;
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
    [Authorize(Roles = RoleGroups.InternalRead)]
    [Route("api/CapaActions")]
    public class CapaActionsController: BaseApiController
    {
        private readonly ICAPAActionsService _actionsService;

        public CapaActionsController(ICAPAActionsService actionsService)
        {
            _actionsService = actionsService;
        }

        [HttpGet("{capaNo}/actions")]
        public async Task<IActionResult> GetActions(string capaNo)
        {
            var capaActions = await _actionsService.GetByCapaNoAsync(capaNo, CompanyId);
            return Ok(capaActions);
        }
        [HttpPost("{capaNo}/actions")]
        [Authorize(Roles = RoleGroups.ComplaintWriteRoles)]
        public async Task<IActionResult> CreateAction(string capaNo, [FromBody] CreateCAPAActionDTO dto) 
        {
            var created = await _actionsService.CreateActionAsync(capaNo, dto, UserId, CompanyId);
            return Ok(created);
        }
        [HttpPatch("actions/{actionId:long}")]
        [Authorize(Roles = RoleGroups.ComplaintWriteRoles)]
        public async Task<IActionResult> UpdateAction(long actionId, [FromBody] UpdateCAPAActionDTO dto) 
        {
            var updated = await _actionsService.UpdateActionAsync(actionId, dto, UserId, CompanyId);
            return Ok(updated);
        }
        [HttpPost("actions/{actionId:long}/complete")]
        [Authorize(Roles = RoleGroups.ComplaintWriteRoles)]
        public async Task<IActionResult> CompleteAction(long actionId, [FromBody] string? note) 
        {
            var dto = new UpdateCAPAActionDTO
            {
                CompletionNote = note,
                Status = "TAMAMLANDI"
            };
            var updated = await _actionsService.UpdateActionAsync(actionId, dto, UserId, CompanyId);
            return Ok(updated);
        }
		[HttpPost("actions/{actionId:long}/files")]
		[Authorize(Roles = RoleGroups.ComplaintWriteRoles)]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> UploadActionFile(long actionId, [FromForm] CreateActionFilesDTO dto)
		{
			var created = await _actionsService.UploadActionFileAsync(actionId, dto, UserId);
			return Ok(created);
		}
		[HttpGet("actions/{actionId:long}/files/{fileId:long}/download")]
		[Authorize(Roles = RoleGroups.InternalRead)]
		public async Task<IActionResult> DownloadActionFile(long actionId, long fileId)
		{
			var result = await _actionsService
				.DownloadActionFileAsync(actionId, fileId);

			return PhysicalFile(
				result.FullPath,
				result.ContentType,
				result.FileName
			);
		}
	}
}
