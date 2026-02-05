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
		[Authorize(Roles = RoleGroups.ContentWrite)]
		[HttpPost("CreateCalibrationFile")]
		public async Task<IActionResult> CreateCalibrationFile([FromBody] Entities.DTOs.InstrumentCalibrationDTOs.CreateCalibrationFileDTO createCalibrationFileDTO)
		{

			var result = await _calibrationFileService.CreateAsync(createCalibrationFileDTO, UserId);
			return Ok();
		}
		[Authorize(Roles = RoleGroups.ContentWrite)]
		[HttpPost("upload")]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> UploadCalibrationFile([FromForm] Entities.DTOs.InstrumentCalibrationDTOs.UploadCalibrationFileDTO uploadCalibrationFileDTO)
		{
			var result = await _calibrationFileService.UploadAsync(uploadCalibrationFileDTO, UserId);
			return Ok();
		}
		[HttpGet("download/{fileId}")]
		public async Task<IActionResult> DownloadCalibrationFile(ulong fileId, [FromQuery] bool asPdf = false)
		{
			var (stream, contentType, name) = await _calibrationFileService.DownloadAsync(fileId, asPdf);
			return File(stream, contentType, name);
		}
		[Authorize(Roles = RoleGroups.ContentWrite)]
		[HttpDelete("delete/{fileId}")]
		public async Task<IActionResult> DeleteCalibrationFile([FromRoute] ulong fileId, [FromQuery] bool deletePhysicalFiles = false)
		{
			var result = await _calibrationFileService.DeleteAsync(fileId, UserId, deletePhysicalFiles);
			if (!result)
			{
				return NotFound();
			}
			return Ok();
		}
	}
}
