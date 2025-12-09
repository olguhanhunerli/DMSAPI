using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs;
using DMSAPI.Entities.DTOs.CategoryDTOs;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;

namespace DMSAPI.Services
{
	public class CategoryServices : ICategoryServices
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public CategoryServices(
			ICategoryRepository categoryRepository,
			IMapper mapper,
			IUserRepository userRepository)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
			_userRepository = userRepository;
		}

		public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO dto, int userId)
		{
			bool exists = await _categoryRepository.ExistsAsync(dto.Name, dto.ParentId, dto.CompanyId);
			if (exists)
				throw new Exception("Category already exists");

			var category = _mapper.Map<Category>(dto);
			category.CreatedAt = DateTime.UtcNow;
			category.IsActive = true;
			category.IsDeleted = false;
			category.CreatedBy = userId;

			await _categoryRepository.AddAsync(category);
			return _mapper.Map<CategoryDTO>(category);
		}

		public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
		{
			var categories = await _categoryRepository.GetCategoriesAsync();
			return _mapper.Map<IEnumerable<CategoryDTO>>(categories.Where(x => !x.IsDeleted));
		}

		public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
		{
			var category = await _categoryRepository.GetCategoryWithChildrenAsync(id)
				?? throw new Exception("Category not found");

			return _mapper.Map<CategoryDTO>(category);
		}

		public async Task<IEnumerable<CategoryTreeDTO>> GetCategoryTreeAsync(int companyId)
		{
			var categories = await _categoryRepository.GetAllAsync();

			var companyCategories = categories
				.Where(c => c.CompanyId == companyId && !c.IsDeleted)
				.ToList();

			var roots = companyCategories.Where(c => c.ParentId == null);

			return roots.Select(c => BuildTree(c, companyCategories)).ToList();
		}

		public async Task<IEnumerable<CategoryDTO>> SearchCategoryTreeAsync(string keyword, int companyId)
		{
			var categories = await _categoryRepository.GetAllAsync();

			var filtered = categories
				.Where(x =>
					x.CompanyId == companyId &&
					!x.IsDeleted &&
					x.Name.Contains(keyword)
				);

			return _mapper.Map<IEnumerable<CategoryDTO>>(filtered);
		}

		public async Task<CategoryDTO> UpdateCategoryByIdAsync(UpdateCategoryDTO dto, int userId)
		{
			var category = await _categoryRepository.GetByIdAsync(dto.Id)
				?? throw new Exception("Category not found");

			_mapper.Map(dto, category);

			category.UpdatedAt = DateTime.UtcNow;
			category.UpdatedBy = userId;

			await _categoryRepository.UpdateAsync(category);
			return _mapper.Map<CategoryDTO>(category);
		}

		public async Task<bool> SoftDeleteCategoryAsync(CategoryDeleteDTO dto, int userIdFromToken)
		{
			await _categoryRepository.SoftDeleteAsync(dto.Id, userIdFromToken);
			return true;
		}

		public async Task<bool> RestoreCategoryAsync(CategoryRestoreDTO dto, int userId)
		{
			await _categoryRepository.RestoreCategoryAsync(dto.Id, userId);
			return true;
		}

		public async Task<List<string>> GetCategoryBreadcrumbAsync(int categoryId)
		{
			var list = new List<string>();
			var category = await _categoryRepository.GetByIdAsync(categoryId);

			while (category != null)
			{
				list.Insert(0, category.Name);
				if (!category.ParentId.HasValue) break;
				category = await _categoryRepository.GetByIdAsync(category.ParentId.Value);
			}

			return list;
		}

		public async Task<CategoryBreadcrumbDTO> GetCategoryBreadcrumbDetailedAsync(int categoryId)
		{
            var breadcrumb = await GetCategoryBreadcrumbAsync(categoryId);

            return new CategoryBreadcrumbDTO
            {
                FullPath = string.Join(" / ", breadcrumb),
                BreadcrumbList = breadcrumb.Select((x, i) => new BreadCrumbItemDTO
                {
                    Id = i,
                    Name = x
                }).ToList()
            };
        }

		public async Task<IEnumerable<CategorySelectListDTO>> GetCategorySelectListAsync()
		{
			var categories = await _categoryRepository.GetAllAsync();

			return categories
				.Where(x => !x.IsDeleted)
				.Select(x => new CategorySelectListDTO
				{
					Id = x.Id,
					DisplayName = x.Name
				});
		}

		private CategoryTreeDTO BuildTree(Category category, IEnumerable<Category> all)
		{
			return new CategoryTreeDTO
			{
				Id = category.Id,
				Name = category.Name,
				ParentId = category.ParentId,
				Children = all
					.Where(x => x.ParentId == category.Id && !x.IsDeleted)
					.Select(x => BuildTree(x, all))
					.ToList()
			};
		}

        public async Task<PagedResultDTO<CategoryDTO>> GetPagedAsync(int page, int pageSize)
        {
            var pagedEntities = await _categoryRepository.GetPagedAsync(page, pageSize);

            return new PagedResultDTO<CategoryDTO>
            {
                TotalCount = pagedEntities.TotalCount,
                Page = pagedEntities.Page,
                PageSize = pagedEntities.PageSize,
                Items = _mapper.Map<List<CategoryDTO>>(pagedEntities.Items)
            };
        }
    }
}
