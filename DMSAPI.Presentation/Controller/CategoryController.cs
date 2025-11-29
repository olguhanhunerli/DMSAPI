using DMSAPI.Entities.DTOs.CategoryDTOs;
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
	public class CategoryController: ControllerBase
	{
		private readonly ICategoryServices _categoryServices;

		public CategoryController(ICategoryServices categoryServices)
		{
			_categoryServices = categoryServices;
		}
		[HttpGet("GetAllCategories")]
		public async Task<IActionResult> GetAllCategories()
		{
			var categories = await _categoryServices.GetAllCategoriesAsync();
			return Ok(categories);
		}
		[HttpGet("GetCategoryById/{id}")]
		public async Task<IActionResult> GetCategoryById(int id)
		{
			var category = await _categoryServices.GetCategoryByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			return Ok(category);
		}
		[HttpGet("GetCategoryCompanyById/{id}")]
		public async Task<IActionResult> GetCategoriesByCompanyId(int id)
		{
			var categories = await _categoryServices.GetCategoriesByCompanyIdAsync(id);
			return Ok(categories);
		}
		[HttpGet(("GetCategoryTree/{companyId}"))]
		public async Task<IActionResult> GetCategoryTree(int companyId)
		{
			var categoryTree = await _categoryServices.GetCategoryTreeAsync(companyId);
			return Ok(categoryTree);
		}
		[HttpPost("SearchCategoryTree")]
		public async Task<IActionResult> SearchCategoryTree([FromBody] CategorySearchDTO categorySearchDTO)
		{
			var searchResults = await _categoryServices.SearchCategoryTreeAsync(categorySearchDTO.Keyword, categorySearchDTO.CompanyId);
			return Ok(searchResults);
		}
		[HttpPost("Create-main-category")]
		public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryDTO categoryDto)
		{
			try
			{
				var createdCategory = await _categoryServices.CreateCategoryAsync(categoryDto);
				return Ok(createdCategory);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	
		[HttpPut("Update-category")]
		public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDTO categoryDto)
		{
			try
			{
				var updatedCategory = await _categoryServices.UpdateCategoryByIdAsync(categoryDto);
				return Ok(updatedCategory);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
