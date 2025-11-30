using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
	public class DocumentService : IDocumentService
	{
		private readonly IDocumentRepository _documentRepository;
		private readonly IMapper _mapper;
		private readonly ICategoryRepository _categoryRepository;
		private readonly ICompanyRepository _companyRepository;
		private readonly IUserRepository _userRepository;
		private readonly IHostEnvironment _env;

		public DocumentService(IDocumentRepository documentRepository, IMapper mapper, ICategoryRepository categoryRepository, ICompanyRepository companyRepository, IUserRepository userRepository, IHostEnvironment env)
		{
			_documentRepository = documentRepository;
			_mapper = mapper;
			_categoryRepository = categoryRepository;
			_companyRepository = companyRepository;
			_userRepository = userRepository;
			_env = env;
		}

		public async Task<DocumentCreateResponseDTO> CreateDocumentAsync(DocumentCreateDTO documentCreateDTO)
		{
			var category = await _categoryRepository.GetByIdAsync(documentCreateDTO.CategoryId);
			if (category == null)
				throw new Exception("Invalid CategoryId");

			var company = await _companyRepository.GetByIdAsync(documentCreateDTO.CompanyId);
			if (company == null)
				throw new Exception("Invalid CompanyId");

			var user = await _userRepository.GetByIdAsync(documentCreateDTO.CreatedBy);
			if (user == null)
				throw new Exception("Invalid CreatedBy UserId");

			int docNumber = await _documentRepository.GetNextDocumentNumberAsync(
				documentCreateDTO.CompanyId,
				documentCreateDTO.CategoryId
			);

			var breadcrumbCategories = await BuildBreadcrumb(category.Id);
			string lastCategoryCode = breadcrumbCategories.Last().Code;

			string documentCode = $"{company.CompanyCode}-{lastCategoryCode}-{docNumber:D3}";

			if (await _documentRepository.DocumentCodeExistingAsync(documentCode))
				throw new Exception("Document code already exists");

			string uploadPath = Path.Combine(
				_env.ContentRootPath,
				"uploads",
				company.CompanyCode,
				lastCategoryCode,
				documentCode
			);

			if (!Directory.Exists(uploadPath))
				Directory.CreateDirectory(uploadPath);

			// 9) Dosya adı: {DocumentCode}_{Title}.ext   -> boşluklar _ ile
			string extension = Path.GetExtension(documentCreateDTO.File.FileName);
			string sanitizedTitle = (documentCreateDTO.Title ?? "Document").Replace(" ", "_");
			string storedFileName = $"{documentCode}_{sanitizedTitle}{extension}";

			string fullFilePath = Path.Combine(uploadPath, storedFileName);

			using (var stream = new FileStream(fullFilePath, FileMode.Create))
			{
				await documentCreateDTO.File.CopyToAsync(stream);
			}

			var document = new Document
			{
				Title = documentCreateDTO.Title,
				DocumentCode = documentCode,

				FileName = storedFileName,
				OriginalFileName = documentCreateDTO.File.FileName,
				FileSize = documentCreateDTO.File.Length,
				FileType = documentCreateDTO.File.ContentType,

				CompanyId = documentCreateDTO.CompanyId,
				CategoryId = documentCreateDTO.CategoryId,

				VersionNumber = 1,
				VersionNote = documentCreateDTO.VersionNote,
				IsLatestVersion = true,

				DocumentType = documentCreateDTO.DocumentType,
				IsPublic = documentCreateDTO.IsPublic,

				AllowedDepartments = string.Join(",", documentCreateDTO.AllowedDepartmentIds ?? new List<int>()),
				AllowedRoles = string.Join(",", documentCreateDTO.AllowedRoleIds ?? new List<int>()),
				AllowedUsers = string.Join(",", documentCreateDTO.AllowedUserIds ?? new List<int>()),

				Status = "Pending",
				CreatedBy = documentCreateDTO.CreatedBy,
				CreatedAt = DateTime.UtcNow
			};

			await _documentRepository.AddAsync(document);

			var response = new DocumentCreateResponseDTO
			{
				Id = document.Id,
				Title = document.Title,
				DocumentCode = document.DocumentCode,

				FileName = storedFileName,
				OriginalFileName = document.OriginalFileName,
				FileType = document.FileType,
				FileSize = document.FileSize,

				CompanyId = company.Id,
				CompanyName = company.Name,
				CategoryId = category.Id,
				CategoryName = category.Name,

				VersionNumber = document.VersionNumber,
				VersionNote = document.VersionNote,
				CreatedAt = document.CreatedAt,
				CreatedBy = document.CreatedBy,
				CreatedByName = user.FirstName,

				Breadcrumb = breadcrumbCategories.Select(c => c.Name).ToList(),
				BreadcrumbPath = string.Join(" / ", breadcrumbCategories.Select(c => c.Name))
			};

			return response;
		}

		public async Task<IEnumerable<DocumentDTO>> GetAllDocumentsAsync()
		{
			var documents = await _documentRepository.GetAllAsync();
			var documentDTOs = _mapper.Map<IEnumerable<DocumentDTO>>(documents);
			return documentDTOs;
		}
		private async Task<List<Category>> BuildBreadcrumb(int categoryId)
		{
			var list = new List<Category>();
			var cat = await _categoryRepository.GetByIdAsync(categoryId);

			while (cat != null)
			{
				list.Insert(0, cat);
				cat = cat.ParentId != null
					? await _categoryRepository.GetByIdAsync(cat.ParentId.Value)
					: null;
			}

			return list;
		}
	}
}
