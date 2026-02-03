using DMSAPI.Entities.DTOs.CAPADTO;
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
    [Authorize]
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
            if(entity == null)
            {  return NotFound(); }
            return Ok(entity);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCAPA(CreateCAPADTO dto)
        {
            var result = await _capaServices.CreateCapaAsync(dto, UserId, CompanyId);
            return Ok(result);
        }
    }
}
