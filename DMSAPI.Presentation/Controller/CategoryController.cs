using DMSAPI.Entities.DTOs.CategoryDTOs;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
                var userIdFromToken = GetUserId();
                var updatedCategory = await _categoryServices.UpdateCategoryByIdAsync(categoryDto,userIdFromToken);
				return Ok(updatedCategory);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		[HttpDelete("SoftDelete-category")]
		public async Task<IActionResult> SoftDeleteCategory([FromBody] CategoryDeleteDTO categoryDeleteDTO)
		{
			try
			{
                var userIdFromToken = GetUserId();
                bool result = await _categoryServices.SoftDeleteCategoryAsync(categoryDeleteDTO);
				if (result)
				{
					return Ok(new { message = "Category soft-deleted successfully." });
				}
				else
				{
					return BadRequest(new { message = "Failed to soft-delete category." });
				}
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		[HttpPut("Restore-category")]
		public async Task<IActionResult> RestoreCategory([FromBody] CategoryRestoreDTO categoryRestoreDTO)
		{
			try
			{
                var userIdFromToken = GetUserId();
                bool result = await _categoryServices.RestoreCategoryAsync(categoryRestoreDTO);
				if (result)
				{
					return Ok(new { message = "Category restored successfully." });
				}
				else
				{
					return BadRequest(new { message = "Failed to restore category." });
				}
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		[HttpGet("GetCategoryBreadcrumb/{categoryId}")]
		public async Task<IActionResult> GetCategoryBreadcrumb(int categoryId)
		{
			try
			{
				var breadcrumb = await _categoryServices.GetCategoryBreadcrumbAsync(categoryId);
				return Ok(breadcrumb);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		[HttpGet("GetCategoryBreadcrumbDetailed/{categoryId}")]
		public async Task<IActionResult> GetCategoryBreadcrumbDetailed(int categoryId)
		{
			try
			{
				var breadcrumb = await _categoryServices.GetCategoryBreadcrumbDetailedAsync(categoryId);
				return Ok(breadcrumb);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		[HttpGet("GetCategorySelectList/{companyId}")]
		public async Task<IActionResult> GetCategorySelectList(int companyId)
		{
			try
			{
                var selectList = await _categoryServices.GetCategorySelectList(companyId);
				return Ok(selectList);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
        private int GetUserId()
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userId == null)
                throw new Exception("User ID not found in token");

            return int.Parse(userId);
        }
    }
}
