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
	public class InstrumentController: BaseApiController
    {
        private readonly IInstrumentServices _instrumentServices;
		public InstrumentController(IInstrumentServices instrumentServices)
		{
			_instrumentServices = instrumentServices;
		}
		[HttpGet("get-instruments")]
		public async Task<IActionResult> GetInstruments(int page =1 , int pageSize = 10)
		{
			var instruments = await _instrumentServices.GetPagedAsync(page,pageSize);
			return Ok(instruments);
		}
		[HttpGet("get-instruments-deleted")]
		public async Task<IActionResult> GetDeletedInstruments(int page = 1, int pageSize = 10)
		{
			var instruments = await _instrumentServices.GetDeletedByPagedAsync(page, pageSize);
			return Ok(instruments);
		}
		[HttpGet("get-instrument-by-id/{id}")]
		public async Task<IActionResult> GetInstrumentById(int id)
		{
			var instrument = await _instrumentServices.GetByIdAsync(id);
			if (instrument == null)
				return NotFound();
			return Ok(instrument);
		}
		[HttpGet("get-instrument-by-deleted-id/{id}")]
		public async Task<IActionResult> GetDeletedInstrumentById(int id)
		{
			var instrument = await _instrumentServices.GetDeletedByIdAsync(id);
			if (instrument == null)
				return NotFound();
			return Ok(instrument);
		}
		[HttpPost("create-instrument")]
		public async Task<IActionResult> CreateInstrument([FromBody] Entities.DTOs.InstrumentDTO.CreateInstrumentDTO createInstrumentDTO)
		{
			await _instrumentServices.CreateAsync(createInstrumentDTO, UserId);
			return Ok(createInstrumentDTO);
		}
		[HttpPut("update-instrument/{id}")]
		public async Task<IActionResult> UpdateInstrument(int id, [FromBody] Entities.DTOs.InstrumentDTO.UpdateInstrumentDTO updateInstrumentDTO)
		{
			var updatedInstrument = await _instrumentServices.UpdateAsync(id, updateInstrumentDTO, UserId);
			return Ok(updatedInstrument);
		}
		[HttpPatch("toggle-instrument-active/{id}")]
		public async Task<IActionResult> ToggleInstrumentActive(int id, [FromQuery] bool isActive)
		{
			await _instrumentServices.ToggleIsActiveAsync(id, isActive, UserId);
			return NoContent();
		}
		[HttpDelete("delete-instrument/{id}")]
		public async Task<IActionResult> DeleteInstrument(int id)
		{
			await _instrumentServices.DeleteAsync(id, UserId);
			return NoContent();
		}
		[HttpPost("backup-delete-instrument/{id}")]
		public async Task<IActionResult> BackupDeleteInstrument(int id)
		{
			await _instrumentServices.BackupDeleteAsync(id);
			return NoContent();
		}
	}
}
