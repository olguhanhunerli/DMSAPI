using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs;
using DMSAPI.Entities.DTOs.CategoryDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
	public class CategoryServices : ICategoryServices
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;

		public CategoryServices(ICategoryRepository categoryRepository, IMapper mapper)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
		}

		public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO categoryDto)
		{
			bool exists = await _categoryRepository.ExistsAsync(categoryDto.Name, categoryDto.CompanyId, categoryDto.ParentId);
			if (exists)
			{
				throw new Exception("A category with the same name already exists in this company and parent category.");
			}
			if(categoryDto.ParentId.HasValue)
			{
				var parentCategory = await _categoryRepository.GetByIdAsync(categoryDto.ParentId.Value);
				if (parentCategory == null)
				{
					throw new Exception("Parent category not found.");
				}
			}
			string slug = GenerateSlug(categoryDto.Name);

			var category = new Category
			{
				Name = categoryDto.Name,
				Description = categoryDto.Description,
				Slug = slug,
				Code = categoryDto.Code,
				ParentId = categoryDto.ParentId,
				CompanyId = categoryDto.CompanyId,
				SortOrder = (int)categoryDto.SortOrder,
				CreatedAt = DateTime.UtcNow,
				IsActive = true,
				IsDeleted = false
			};
			await _categoryRepository.AddAsync(category);
			return _mapper.Map<CategoryDTO>(category);
		}

		public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
		{
			var categories = await _categoryRepository.GetAllAsync();
			categories = categories.Where(c => !c.IsDeleted);
			return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
		}

		public async Task<IEnumerable<CategoryDTO>> GetCategoriesByCompanyIdAsync(int companyId)
		{
			var categories = await _categoryRepository.GetCategoriesByCompanyId(companyId);
			return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
		}

		public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
		{
			var category = await _categoryRepository.GetCategoryWithChildrenAsync(id);
			return _mapper.Map<CategoryDTO>(category);
		}

		public async Task<IEnumerable<CategoryTreeDTO>> GetCategoryTreeAsync(int companyId)
		{
			var categories = await _categoryRepository.GetCategoriesByCompanyId(companyId);

			var roots = categories.Where(c => c.ParentId == null);
			return roots.Select(c => BuildTree(c, categories)).ToList();
		}

		public async Task<CategoryDTO> UpdateCategoryByIdAsync(UpdateCategoryDTO categoryDto)
		{
			var category = await _categoryRepository.GetByIdAsync(categoryDto.Id);
			if (category == null)
			{
				throw new Exception("Category not found.");
			}
			if (!string.IsNullOrEmpty(category.Name))
			{
				bool exists = (await _categoryRepository.FindAsync(
						c => c.Id != categoryDto.Id &&
							 c.Name == categoryDto.Name &&
							 c.ParentId == (categoryDto.ParentId ?? category.ParentId) &&
							 !c.IsDeleted
					)).Any();
				if (exists)
				{
					throw new Exception("A category with the same name already exists in this company and parent category.");
				}
			}
			if (categoryDto.ParentId.HasValue)
			{
				var parentCategory = await _categoryRepository.GetByIdAsync(categoryDto.ParentId.Value);
				if (parentCategory == null)
				{
					throw new Exception("Parent category not found.");
				}
			}
			if (categoryDto.Name != null)
			{
				category.Name = categoryDto.Name;
				category.Slug = GenerateSlug(categoryDto.Name);
			}
			if (categoryDto.Description != null)
			{
				category.Description = categoryDto.Description;
			}
			if (categoryDto.ParentId.HasValue)
			{
				category.ParentId = categoryDto.ParentId;
			}
			if (categoryDto.SortOrder.HasValue)
			{
				category.SortOrder = categoryDto.SortOrder.Value;
			}
			if (categoryDto.IsActive.HasValue)
			{
				category.IsActive = categoryDto.IsActive.Value;
			}
			category.UpdatedAt = DateTime.UtcNow;

			await _categoryRepository.UpdateAsync(category);
			return _mapper.Map<CategoryDTO>(category);
		}
		public async Task<IEnumerable<CategoryDTO>> SearchCategoryTreeAsync(string keyword, int companyId)
		{
			var categories = await _categoryRepository.GetCategoriesByCompanyId(companyId);
			var dtoList = _mapper.Map<IEnumerable<CategoryDTO>>(categories);

			var matched = dtoList
				.Where(c => c.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
				.ToList();
			var resultTree = new List<CategoryDTO>();
			foreach (var category in matched)
			{
				AddNodeWithParents(category, dtoList, resultTree);
			}
			return resultTree;
		}
		public async Task<bool> RestoreCategoryAsync(CategoryRestoreDTO categoryRestoreDTO)
		{
			await _categoryRepository.RestoreCategoryAsync(categoryRestoreDTO.Id, categoryRestoreDTO.UploadedBy);
			return true;

		}
		public async Task<bool> SoftDeleteCategoryAsync(CategoryDeleteDTO categoryDeleteDTO)
		{
			await _categoryRepository.SoftDeleteAsync(categoryDeleteDTO.Id, categoryDeleteDTO.UploadedBy);
			return true;

		}
		public async Task<List<string>> GetCategoryBreadcrumbAsync(int categoryId)
		{
			var category = await _categoryRepository.GetByIdAsync(categoryId);
			if (category == null)
			{
				throw new Exception("Category not found.");
			}
			List<string> breadcrumb = new List<string>();
			while (category != null)
			{
				breadcrumb.Insert(0, category.Name);
				if (category.ParentId == null) break;
				category = await _categoryRepository.GetByIdAsync(category.ParentId.Value);
			}
			return breadcrumb;
		}
		public async Task<CategoryBreadcrumbDTO> GetCategoryBreadcrumbDetailedAsync(int categoryId)
		{
			var list = new List<Category>();

			var category = await _categoryRepository.GetByIdAsync(categoryId);
			if (category == null)
			{
				throw new Exception("Category not found.");
			}
			list.Add(category);
			while (category.ParentId.HasValue)
			{
				category = await _categoryRepository.GetByIdAsync(category.ParentId.Value);
				if (category == null) break;
				list.Add(category);
			}
			list.Reverse();
			return new CategoryBreadcrumbDTO
			{
				BreadcrumbList = list.Select(c => new BreadCrumbItemDTO
				{
					Id = c.Id,
					Name = c.Name,
				}).ToList(),
				FullPath = string.Join(" / ", list.Select(c => c.Name))
			};

		}
		public async Task<IEnumerable<CategorySelectListDTO>> GetCategorySelectList(int companyId)
		{
			var categories = await _categoryRepository.GetCategoriesByCompanyId(companyId);
			var list = new List<CategorySelectListDTO>();
			foreach (var category in categories)
			{
				var breadcrumb = await GetCategoryBreadcrumbAsync(category.Id);
				string displayName = string.Join(" / ", breadcrumb);
				list.Add(new CategorySelectListDTO
				{
					Id = category.Id,
					DisplayName = displayName
				});
				
			}
			return list.OrderBy(c => c.DisplayName);
		}
		private string GenerateSlug(string name)
		{
			return name.ToLower().Replace(" ", "-");
		}
		
		private CategoryTreeDTO BuildTree(Category category, IEnumerable<Category> allCategories)
		{
			var node = new CategoryTreeDTO
			{
				Id = category.Id,
				Name = category.Name,
				Description = category.Description,
				ParentId = category.ParentId,
				CompanyId = category.CompanyId,
				SortOrder = category.SortOrder,
				Children = new List<CategoryTreeDTO>()
			};

			var children = allCategories
				.Where(c => c.ParentId == category.Id && !c.IsDeleted)
				.OrderBy(c => c.SortOrder)
				.ToList();
			node.Children = children.Any()
				? children.Select(child => BuildTree(child, allCategories)).ToList()
				: new List<CategoryTreeDTO>();
			return node;
		}
		
		private void AddNodeWithParents(CategoryDTO category, IEnumerable<CategoryDTO> allCategories, List<CategoryDTO> resultTree)
		{
			if (category.ParentId == null)
			{
				if (!resultTree.Any(c => c.Id == category.Id))
				{
					resultTree.Add(category);
				}
				return;
			}
			var parent = allCategories.FirstOrDefault(c => c.Id == category.ParentId);
			if (parent == null)
			{
				return;
			}
			var existingParent = resultTree.FirstOrDefault(c => c.Id == parent.Id);
			if(existingParent == null)
			{
				existingParent = new CategoryDTO
				{
					Id = parent.Id,
					Name = parent.Name,
					Description = parent.Description,
					ParentId = parent.ParentId,
					CompanyId = parent.CompanyId,
					SortOrder = parent.SortOrder,
					Children = new List<CategoryDTO>()
				};
				resultTree.Add(existingParent);
			}
			if (!existingParent.Children.Any(c => c.Id == category.Id))
			{
				if(category.Children == null)
				{
					category.Children = new List<CategoryDTO>();
				}
				existingParent.Children.Add(category);
			}
			AddNodeWithParents(parent, allCategories, resultTree);
		}

	
	}
}
