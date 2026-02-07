using DMSAPI.Entities.DTOs.CapaEvidenceFiles;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Presentation.Controller
{
	[Route("api/capa-evidence")]
	public class CapaEvidenceFilesController: BaseApiController
    {
        private readonly ICapaEvidenceService _capaEvidenceService;

		public CapaEvidenceFilesController(ICapaEvidenceService capaEvidenceService)
		{
			_capaEvidenceService = capaEvidenceService;
		}
		[HttpPost]
		public async Task<IActionResult> CreateEvidence(string capaNo, CreateCapaEvidenceFilesDTO dto)
		{
			var created = await _capaEvidenceService.CreateFiles(capaNo, dto, UserId);
			return Ok();
		}
	}
}
