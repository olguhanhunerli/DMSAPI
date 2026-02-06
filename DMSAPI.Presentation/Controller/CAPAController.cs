using DMSAPI.Entities.DTOs.CAPADTO;
using DMSAPI.Presentation.Authorization;
using DMSAPI.Services;
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
    [Route("api/[controller]")]
    public class CAPAController : BaseApiController
    {
        private readonly ICAPAServices _capaServices;

        public CAPAController(ICAPAServices capaServices)
        {
            _capaServices = capaServices;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCapas(int page = 1, int pageSize = 10)
        {
            var entity = await _capaServices.GetCapaPagedResult(page, pageSize);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
        [HttpGet("GetByCapaNo")]
        public async Task<IActionResult> GetCapasByCapaNo(string capaNo)
        {
            var entity = await _capaServices.GetCAPAByCapaNoAsync(capaNo);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
        [HttpGet("create-form")]
        public async Task<IActionResult> GetCreateForm([FromQuery] string complaintNo)
        {
            var userId = UserId;
            var companyId = CompanyId;

            var result = await _capaServices.GetCreateFormInitAsync(complaintNo, userId, companyId);
            return Ok(result);
        }
        [HttpGet("root-couse-method")]
        public async Task<IActionResult> GetRootCauseMethods()
        {
            var entity = await _capaServices.GetRootCouseMethodAsync();
            if (entity == null)
            { return NotFound(); }
            return Ok(entity);
        }
        [Authorize(Roles = DmsRoles.User + "," + DmsRoles.Editor + "," + DmsRoles.Admin + "," + DmsRoles.SUPER_ADMIN + "," + DmsRoles.Approver)]
        [HttpPost]
        public async Task<IActionResult> CreateCAPA(CreateCAPADTO dto)
        {
            var result = await _capaServices.CreateCapaAsync(dto, UserId, CompanyId);
            return Ok(result);
        }
        [HttpPatch("{capaNo}")]
        [Authorize(Roles = RoleGroups.ComplaintWriteRoles)]
        public async Task<IActionResult> UpdateCapa(string capaNo, [FromBody] UpdateCAPADTO dto)
        {
            var updated = await _capaServices.UpdateCapaAsync(capaNo, dto, UserId, CompanyId);
            return Ok(updated);
        }
        [HttpPost("{capaNo}/close")]
        [Authorize(Roles = RoleGroups.ComplaintWriteRoles)]
        public async Task<IActionResult> CloseCapa([FromRoute] string capaNo, [FromBody] ClosedCAPADTO dto)
        {
            var closed = await _capaServices.ClosedCapaAsync(capaNo, dto, UserId);
            return Ok(closed);
        }

    }
}
