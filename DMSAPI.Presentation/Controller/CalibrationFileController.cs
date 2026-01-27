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
	public class CalibrationFileController : BaseApiController
	{
		private readonly ICalibrationFileService _calibrationFileService;

		public CalibrationFileController(ICalibrationFileService calibrationFileService)
		{
			_calibrationFileService = calibrationFileService;
		}
		[HttpGet("GetCalibrationFilesByCalibrationId")]
		public async Task<IActionResult> GetCalibrationFilesByCalibrationId([FromQuery] int pageNumber, [FromQuery] int pageSize)
		{
			var result = await _calibrationFileService.GetCalibrationFilesByCalibrationIdAsync(pageNumber, pageSize);
			return Ok(result);
		}
		[HttpGet("GetCalibrationFileById/{fileId}")]
		public async Task<IActionResult> GetCalibrationFileById([FromRoute] ulong fileId)
		{
			var result = await _calibrationFileService.GetCalibrationFileByIdAsync(fileId);
			if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}
		[HttpPost("CreateCalibrationFile")]
		public async Task<IActionResult> CreateCalibrationFile([FromBody] Entities.DTOs.InstrumentCalibrationDTOs.CreateCalibrationFileDTO createCalibrationFileDTO)
		{

			var result = await _calibrationFileService.CreateAsync(createCalibrationFileDTO, UserId);
			return Ok();
		}
		[HttpPost("upload")]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> UploadCalibrationFile([FromForm] Entities.DTOs.InstrumentCalibrationDTOs.UploadCalibrationFileDTO uploadCalibrationFileDTO)
		{
			var result = await _calibrationFileService.UploadAsync(uploadCalibrationFileDTO, UserId);
			return Ok();
		}
	}
}
