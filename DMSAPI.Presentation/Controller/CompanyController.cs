using DMSAPI.Entities.DTOs.CompanyDTOs;
using DMSAPI.Presentation.Controller;
using DMSAPI.Services;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/[controller]")]
public class CompanyController : BaseApiController
{
	private readonly ICompanyService _service;

	public CompanyController(ICompanyService service)
	{
		_service = service;
	}

	[HttpGet("get-all")]
	public async Task<IActionResult> GetAll()
	{
		var role = User.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
		var companyIdClaim = User.Claims.FirstOrDefault(x => x.Type == "companyId")?.Value;

		if (role == "GLOBAL ADMIN")
		{
			var allCompanies = await _service.GetAllCompaniesAsync();
			return Ok(allCompanies);
		}

		if (int.TryParse(companyIdClaim, out var companyId))
		{
			var company = await _service.GetCompanyByIdAsync(companyId);
			return Ok(new List<CompanyDTO> { company });
		}

		return Forbid();
	}

	[HttpGet("get-by-id/{id}")]
	public async Task<IActionResult> GetById(int id)
		=> Ok(await _service.GetCompanyByIdAsync(id));

	[HttpPost("create")]
	public async Task<IActionResult> Create(AddCompanyDTO dto)
	{
		if (!IsGlobalAdmin) return Forbid();
		await _service.CreateCompanyAsync(dto, UserId);
		return Ok();
	}

	[HttpPut("update/{id}")]
	public async Task<IActionResult> Update(int id, UpdateCompanyDTO dto)
	{
		if (!IsGlobalAdmin) return Forbid();
		await _service.UpdateCompanyAsync(id, dto, UserId);
		return Ok();
	}

	[HttpDelete("delete/{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		if (!IsGlobalAdmin) return Forbid();
		await _service.DeleteCompanyAsync(id, UserId);
		return Ok();
	}
}
