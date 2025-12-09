using AutoMapper;
using DMSAPI.Business.Repositories;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using Microsoft.Extensions.Hosting;

namespace DMSAPI.Services
{
	public class DocumentService : IDocumentService
	{
		private readonly IDocumentRepository _documentRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IHostEnvironment _env;

		public DocumentService(
			IDocumentRepository documentRepository,
			IMapper mapper,
			ICategoryRepository categoryRepository,
			IUserRepository userRepository,
			IHostEnvironment env)
		{
			_documentRepository = documentRepository;
			_mapper = mapper;
			_categoryRepository = categoryRepository;
			_userRepository = userRepository;
			_env = env;
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
