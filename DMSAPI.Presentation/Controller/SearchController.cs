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
	public class SearchController: BaseApiController
    {
        private readonly IGlobalSearchService _globalSearchService;

		public SearchController(IGlobalSearchService globalSearchService)
		{
			_globalSearchService = globalSearchService;
		}
		[HttpGet("global")]
		public async Task<IActionResult> GlobalSearchAsync([FromQuery] string query, int page = 1, int pageSize = 10)
		{
			var userId = UserId;
			var roleId = RoleId;
			var departmentId = DepartmentId;
			var result = await _globalSearchService.GlobalSearchAsync(query, roleId, userId, departmentId, page, pageSize);
			return Ok(result);
		}
	}
}
