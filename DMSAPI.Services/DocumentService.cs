using AutoMapper;
using DMSAPI.Business.Repositories;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using Microsoft.Extensions.Hosting;
using System.Numerics;

namespace DMSAPI.Services
{
	public class DocumentService : IDocumentService
	{
		private readonly IDocumentRepository _documentRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IHostEnvironment _env;
		private readonly ICategoryServices  _categoryServices;

        public DocumentService(
            IDocumentRepository documentRepository,
            IMapper mapper,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IHostEnvironment env,
            ICategoryServices categoryServices)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _env = env;
            _categoryServices = categoryServices;
        }

        public async Task<DocumentCreateResponseDTO> CreateDocumentAsync(DocumentCreateDTO dto, int userId)
		{
			var user = await _userRepository.GetByIdAsync(userId);
			var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);

			int number = await _documentRepository.GetNextDocumentNumberAsync(dto.CompanyId,dto.CategoryId);

			string code = $"{user.Company.CompanyCode}-{category.Code}-{number:D3}";

			if (await _documentRepository.DocumentCodeExistingAsync(code))
				throw new Exception("Document code exists");

			var document = _mapper.Map<Document>(dto);
			document.DocumentCode = code;
			document.CreatedByUserId = userId;
			document.CreatedAt = DateTime.UtcNow;

			await _documentRepository.AddAsync(document);

			return _mapper.Map<DocumentCreateResponseDTO>(document);
		}

		public async Task<IEnumerable<DocumentDTO>> GetAllDocumentsAsync()
		{
			var docs = await _documentRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<DocumentDTO>>(docs);
		}

        public async Task<DocumentCreatePreviewDTO> GetCreatePreviewAsync(int categoryId, int userId)
        {
            var user = await _userRepository.GetUserByIdsync(userId)
                ?? throw new Exception("User Not Found");
            var category = await _categoryRepository.GetByIdAsync(categoryId)
                ?? throw new Exception("Category Not Found");
            var breadcrumbList = await _categoryServices.GetCategoryBreadcrumbDetailedAsync(categoryId);

            var breadcrumbDto =
                         await _categoryServices.GetCategoryBreadcrumbDetailedAsync(categoryId);

            var breadcrumbPath =
                !string.IsNullOrWhiteSpace(breadcrumbDto?.FullPath)
                    ? breadcrumbDto.FullPath
                    : category.Name;

            int number = await _documentRepository
                .GetNextDocumentNumberAsync(user.CompanyId, categoryId);

            string code = $"{user.Company.CompanyCode}-{category.Code}-{number:D3}";

            bool exists = await _documentRepository.DocumentCodeExistingAsync(code);

            return new DocumentCreatePreviewDTO
            {
                DocumentCode = code,
                CompanyName = user.Company.Name,
                CategoryName = category.Name,
                CategoryBreadcrumb = breadcrumbPath,

                VersionNumber = 1,
                Status = "Draft",

                OwnerName = $"{user.FirstName} {user.LastName}",
                CreatedAt = DateTime.UtcNow,

                IsCodeValid = !exists
            };
        }

        public async Task<PagedResultDTO<DocumentDTO>> GetPageAsync(int page, int pageSize, int userId, int roleId, int departmentId)
        {
            var result = await _documentRepository
							.GetPagedAuthorizedAsync(page, pageSize, userId, roleId, departmentId);

            return new PagedResultDTO<DocumentDTO>
            {
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize,
                Items = _mapper.Map<List<DocumentDTO>>(result.Items)
            };
        }
    }
}
