using DMSAPI.Entities.DTOs.InstrumentCalibrationDTOs;
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
	[Authorize(Roles = RoleGroups.MasterDataWrite)]
	[Route("api/[controller]")]
	public class InstrumentCalibrationsController: BaseApiController
    {
        private readonly IInstrumentCalibrationService _instrumentCalibrationService;

		public InstrumentCalibrationsController(IInstrumentCalibrationService instrumentCalibrationService)
		{
			_instrumentCalibrationService = instrumentCalibrationService;
		}
		[HttpGet]
		public async Task<IActionResult> GetInstrumentCalibrations([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			var result = await _instrumentCalibrationService.GetInstrumentCalibrationsAsync(pageNumber, pageSize);
			return Ok(result);
		}
		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> GetById(ulong id)
		{
			var result = await _instrumentCalibrationService.GetByIdASync(id);
			return Ok(result);
		}
		[HttpPost]
		public async Task<IActionResult> CreateCalibration([FromBody] Entities.DTOs.InstrumentCalibrationDTOs.CreateCalibrationDTO createCalibrationDTO)
		{
			var result = await _instrumentCalibrationService.CreateCalibrationAsync(createCalibrationDTO, UserId);
			return Ok(result);
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(ulong id,[FromBody] UpdateCalibrationDTO dto)
		{
			if (id != dto.CalibrationId)
				return BadRequest("Id mismatch");

			await _instrumentCalibrationService.UpdateCalibrationAsync(dto, UserId);
			return Ok(dto);
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(ulong id)
		{
			await _instrumentCalibrationService.DeleteCalibrationAsync(id, UserId);
			return NoContent();
		}
	}
}
