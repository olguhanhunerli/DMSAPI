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
	}
}
