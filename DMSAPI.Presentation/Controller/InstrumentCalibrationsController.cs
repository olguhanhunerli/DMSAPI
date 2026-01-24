using DMSAPI.Services.IServices;
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
	}
}
