using AutoMapper;
using DMSAPI.Business.Repositories;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.DTOs.RoleDTOs;
using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text.Json;

namespace DMSAPI.Services
{
	public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _env;
        private readonly ICategoryServices _categoryServices;
        private readonly IDocumentAttachmentRepository _documentAttachmentRepository;
        private readonly IDocumentApprovalRepository _documentApprovalRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDocumentApprovalHistoryRepository _documentApprovalHistoryRepository;
        private readonly IDocumentCodeReservationRepository _documentCodeReservationRepository;

		public DocumentService(
			IDocumentRepository documentRepository,
			IMapper mapper,
			ICategoryRepository categoryRepository,
			IUserRepository userRepository,
			IHostEnvironment env,
			ICategoryServices categoryServices,
			IDocumentAttachmentRepository documentAttachmentRepository,
			IDocumentApprovalRepository documentApprovalRepository,
			IRoleRepository roleRepository,
			IDepartmentRepository departmentRepository,
			IDocumentApprovalHistoryRepository documentApprovalHistoryRepository,
			IDocumentCodeReservationRepository documentCodeReservationRepository)
		{
			_documentRepository = documentRepository;
			_mapper = mapper;
			_categoryRepository = categoryRepository;
			_userRepository = userRepository;
			_env = env;
			_categoryServices = categoryServices;
			_documentAttachmentRepository = documentAttachmentRepository;
			_documentApprovalRepository = documentApprovalRepository;
			_roleRepository = roleRepository;
			_departmentRepository = departmentRepository;
			_documentApprovalHistoryRepository = documentApprovalHistoryRepository;
			_documentCodeReservationRepository = documentCodeReservationRepository;
		}

		public async Task<DocumentCreateResponseDTO> CreateDocumentAsync(DocumentCreateDTO dto, int userId)
        {
            using var trx = await _documentRepository.BeginTransactionAsync();

            try
            {
                var user = await _userRepository.GetUserByIdsync(userId)
                    ?? throw new Exception("User not found");

                var category = await _categoryRepository.GetByIdAsync(dto.CategoryId)
                    ?? throw new Exception("Category not found");

                int companyId = user.CompanyId;
                var reservation = await _documentCodeReservationRepository.GetByCodeAsync(dto.DocumentCode);
				if (reservation == null || reservation.IsUsed || reservation.CompanyId != companyId || reservation.CategoryId != dto.CategoryId)
					throw new Exception("Invalid or already used document code reservation");

				string documentType =
			        Path.GetExtension(dto.MainFile.FileName)?.ToLower()
			        ?? "unknown";
				var document = new Document
                {
					Title = dto.TitleTr,
					CategoryId = dto.CategoryId,
					CompanyId = user.CompanyId,
					DocumentCode = reservation.DocumentCode, 
					DocumentType = Path.GetExtension(dto.MainFile?.FileName ?? "") ?? "unknown",
					CreatedAt = DateTime.UtcNow,
					CreatedByUserId = userId,
					VersionNumber = 1,
					StatusId = 1,
					VersionNote = dto.VersionNote,
					IsDeleted = false,
					AllowedDepartments = dto.AllowedDepartmentIds != null
			        ? JsonSerializer.Serialize(dto.AllowedDepartmentIds)
			        : null,
					        AllowedRoles = dto.AllowedRoleIds != null
			        ? JsonSerializer.Serialize(dto.AllowedRoleIds)
			        : null,
					        AllowedUsers = dto.AllowedUserIds != null
			        ? JsonSerializer.Serialize(dto.AllowedUserIds)
			        : null
				};

                await _documentRepository.AddAsync(document);
                await _documentCodeReservationRepository.MarkAsUsedAsync(reservation.DocumentCode);
				await _documentApprovalHistoryRepository.AddAsync(new DocumentApprovalHistory
                {
                    DocumentId = document.Id,
                    ActionType = "Created",
                    ActionByUserId = userId,
                    ActionAt = DateTime.UtcNow,
                    ActionNote = "Doküman Oluşturuldu"
                });


                if (dto.MainFile != null)
                {
                    var folder = Path.Combine(
                        _env.ContentRootPath,
                        "uploads",
                        "documents",
                        category.Name,
                        document.DocumentCode,
                        "main"
                    );

                    Directory.CreateDirectory(folder);

					var cleanName = Path.GetFileName(dto.MainFile.FileName);
					var fullPath = Path.Combine(folder, cleanName);

					using (var stream = new FileStream(fullPath, FileMode.Create))
					{
						await dto.MainFile.CopyToAsync(stream);
					}

					ConvertToPdf(fullPath, folder);

					var pdfFileName = Path.GetFileNameWithoutExtension(cleanName) + ".pdf";
					var pdfFullPath = Path.Combine(folder, pdfFileName);

					if (!File.Exists(pdfFullPath))
					{
						throw new Exception("PDF üretilemedi. Doküman oluşturulamadı.");
					}

					var mainFiles = new DocumentFile
                    {
                        DocumentId = document.Id,
                        FileName = cleanName,
                        OriginalFileName = dto.MainFile.FileName,
                        FileExtension = Path.GetExtension(cleanName),
                        FileSize = dto.MainFile.Length,
                        FilePath = $"uploads/documents/{category.Name}/{document.DocumentCode}/main/{cleanName}",
						PdfFilePath = $"uploads/documents/{category.Name}/{document.DocumentCode}/main/{pdfFileName}",
						UploadedAt = DateTime.UtcNow,
                        UploadedByUserId = userId,
                        IsDeleted = false
                    };

                    await _documentRepository.AddMainFileAsync(mainFiles);
                }

                if (dto.Attachments != null)
                {
                    var attachFolder = Path.Combine(
                        _env.ContentRootPath,
                        "uploads",
                        "documents",
                        category.Name,
                        document.DocumentCode,
                        "attachments"
                    );

                    Directory.CreateDirectory(attachFolder);

                    foreach (var file in dto.Attachments)
                    {
                        var cleanName = Path.GetFileName(file.FileName);
                        var fullPath = Path.Combine(attachFolder, cleanName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                            await file.CopyToAsync(stream);

                        var attach = new DocumentAttachment
                        {
                            DocumentId = document.Id,
                            FileName = cleanName,
                            OriginalFileName = file.FileName,
                            FileSize = file.Length,
                            FilePath = $"uploads/documents/{category.Name}/{document.DocumentCode}/attachments/{cleanName}",
                            UploadedAt = DateTime.UtcNow,
                            UploadedByUserId = userId,
                            IsDeleted = false
                        };

                        await _documentAttachmentRepository.AddAsync(attach);
                    }
                }

                if (dto.ApproverUserIds.Any())
                {
                    int level = 1;

                    foreach (var approverId in dto.ApproverUserIds)
                    {
                        await _documentApprovalRepository.AddAsync(new DocumentApproval
                        {
                            DocumentId = document.Id,
                            UserId = approverId,
                            ApprovalLevel = level++,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
                await _documentApprovalHistoryRepository.AddAsync(new DocumentApprovalHistory
                {
                    DocumentId = document.Id,
                    ActionType = "SENT_FOR_APPROVAL",
                    ActionByUserId = userId,
                    ActionAt = DateTime.UtcNow,
                    ActionNote = "Doküman Onay Sürecine Gönderildi"
                });

                await trx.CommitAsync();

				var dtoResponse = _mapper.Map<DocumentCreateResponseDTO>(document);

				var mainFile = await _documentRepository.GetMainFileAsync(document.Id);

				if (mainFile != null)
				{
					dtoResponse.FileName = mainFile.FileName;
					dtoResponse.OriginalFileName = mainFile.OriginalFileName;
					dtoResponse.FileSize = mainFile.FileSize;
					dtoResponse.FileType = mainFile.FileExtension;
				}

				dtoResponse.CompanyName = user.Company.Name;
				dtoResponse.CategoryName = category.Name;

				return dtoResponse;
			}
            catch
            {
                throw;
            }
        }

		public async Task<DownloadFileResultDTO> DownloadDocumentFileAsync(int documentId)
		{
			var document = await _documentRepository.GetDocumentWithFileAsync(documentId)
				?? throw new Exception("Document not found");
            var file = document.Files.First();
            var bytes = await File.ReadAllBytesAsync(file.FilePath);
			return new DownloadFileResultDTO
			{
				FileBytes = bytes,
				OriginalFileName = file.OriginalFileName,
				ContentType = GetContentType(file.FilePath)
			};
		}

		public async Task<DownloadFileResultDTO> DownloadPdfAsync(int documentId)
		{
			var document = await _documentRepository.GetDocumentWithFileAsync(documentId)
				?? throw new Exception("Document not found");
            var file = document.Files.FirstOrDefault()
                ?? throw new Exception("PDF Dosyası Bulunamadı");
			var bytes = await File.ReadAllBytesAsync(file.PdfFilePath);
			return new DownloadFileResultDTO
			{
				FileBytes = bytes,
				OriginalFileName = Path.GetFileName(file.PdfFilePath), 
				ContentType = "application/pdf"                        
			};

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

            var reservation = await _documentCodeReservationRepository
				.ReserveNextCodeAsync(
					user.CompanyId,
					categoryId,
					user.Company.CompanyCode,
					category.Code,
					userId);

			return new DocumentCreatePreviewDTO
            {
				DocumentCode = reservation!.DocumentCode,
				CompanyName = user.Company.Name,
				CategoryName = category.Name,
				CategoryBreadcrumb = breadcrumbPath,
				VersionNumber = 1,
				Status = "Draft",
				OwnerName = $"{user.FirstName} {user.LastName}",
				CreatedAt = DateTime.UtcNow,
				IsCodeValid = true
			};
        }

		public async Task<DocumentDTO> GetDetailByIdAsync(int documentId)
		{
			var document = await _documentRepository.GetDetailByIdAsync(documentId)
				?? throw new Exception("Document not found");
            var dto = _mapper.Map<DocumentDTO>(document);

            await EnrichAllowedEntitiesAsync(new[] { dto });

            return dto;
        }

		public async Task<List<DocumentDTO>> GetMyPendingApprovalsAsync(int userId)
        {
            var documentIds = await _documentApprovalRepository.GetPendingDocumentIdsAsync(userId);
            if (!documentIds.Any())
            {
                return new List<DocumentDTO> ();
            }
            var documents = await _documentRepository.GetPendingDocumentIdsForUserAsync(documentIds);

            return _mapper.Map <List<DocumentDTO>> (documents);
        }

        public async Task<PagedResultDTO<MyPendingDTO>> GetMyPendingApprovalsAsync(
                                                                             int page,
                                                                             int pageSize,
                                                                             int userId)
        {
            var documentIds =
                await _documentApprovalRepository
                    .GetPendingDocumentIdsForUserAsync(userId);

            if (!documentIds.Any())
            {
                return new PagedResultDTO<MyPendingDTO>
                {
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize,
                    Items = new List<MyPendingDTO>()
                };
            }

            var pagedDocuments =
                await _documentRepository
                    .GetPagedPendingByIdsAsync(documentIds, page, pageSize);

            return new PagedResultDTO<MyPendingDTO>
            {
                TotalCount = pagedDocuments.TotalCount,
                Page = pagedDocuments.Page,
                PageSize = pagedDocuments.PageSize,
                Items = _mapper.Map<List<MyPendingDTO>>(pagedDocuments.Items)
            };
        }
        public async Task<PagedResultDTO<DocumentDTO>> GetPageAsync(int page, int pageSize, int userId, int roleId, int departmentId)
        {
            var result = await _documentRepository
                            .GetPagedAuthorizedAsync(page, pageSize, userId, roleId, departmentId);
            var items = _mapper.Map<List<DocumentDTO>>(result.Items);
            await EnrichAllowedEntitiesAsync(items);
            return new PagedResultDTO<DocumentDTO>
            {
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize,
                Items = items
            };
        }

		public async Task<PagedResultDTO<DocumentDTO>> GetPagedApprovedAsync(int page, int pageSize, int userId, int roleId, int departmentId)
		{
			var result = await _documentRepository.GetPagedApprovedAsync(page, pageSize, userId, roleId,departmentId);

			return new PagedResultDTO<DocumentDTO>
			{
				TotalCount = result.TotalCount,
				Page = result.Page,
				PageSize = result.PageSize,
				Items = _mapper.Map<List<DocumentDTO>>(result.Items)
			};
		}

		public async Task<PagedResultDTO<DocumentDTO>> GetRejectedDocumentsAsync(int page, int pageSize)
		{
			var pagedDocs = await _documentRepository
		        .GetPagedRejectedAsync(page, pageSize);

			return new PagedResultDTO<DocumentDTO>
			{
				TotalCount = pagedDocs.TotalCount,
				Page = pagedDocs.Page,
				PageSize = pagedDocs.PageSize,
				Items = _mapper.Map<List<DocumentDTO>>(pagedDocs.Items)
			};
		}

		private void ConvertToPdf(string inputFilePath, string outputFolder)
		{
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = @"C:\Program Files\LibreOffice\program\soffice.exe",
					Arguments = $"--headless --convert-to pdf \"{inputFilePath}\" --outdir \"{outputFolder}\"",
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};

			process.Start();
			process.WaitForExit();
		}
		private static string GetContentType(string path)
		{
			var provider = new FileExtensionContentTypeProvider();

			if (!provider.TryGetContentType(path, out var contentType))
			{
				contentType = "application/octet-stream";
			}

			return contentType;
		}
        private async Task EnrichAllowedEntitiesAsync(IEnumerable<DocumentDTO> documents)
        {
            if (documents == null || !documents.Any())
                return;

            var roleIds = documents
                .Where(d => d.AllowedRoleIds != null)
                .SelectMany(d => d.AllowedRoleIds!)
                .Distinct()
                .ToList();

            var departmentIds = documents
                .Where(d => d.AllowedDepartmentIds != null)
                .SelectMany(d => d.AllowedDepartmentIds!)
                .Distinct()
                .ToList();

            var userIds = documents
                .Where(d => d.AllowedUserIds != null)
                .SelectMany(d => d.AllowedUserIds!)
                .Distinct()
                .ToList();

            var roles = roleIds.Any()
                ? await _roleRepository.GetByIdsAsync(roleIds)
                : new List<Role>();

            var departments = departmentIds.Any()
                ? await _departmentRepository.GetByIdsAsync(departmentIds)
                : new List<Department>();

            var users = userIds.Any()
                ? await _userRepository.GetByIdsAsync(userIds)
                : new List<User>();

            var roleDict = roles.ToDictionary(x => x.Id);
            var departmentDict = departments.ToDictionary(x => x.Id);
            var userDict = users.ToDictionary(x => x.Id);

            foreach (var dto in documents)
            {
                if (dto.AllowedRoleIds?.Any() == true)
                {
                    dto.AllowedRoles = dto.AllowedRoleIds
                        .Where(id => roleDict.ContainsKey(id))
                        .Select(id => new RoleDTO
                        {
                            Id = roleDict[id].Id,
                            Name = roleDict[id].Name
                        })
                        .ToList();
                }

                if (dto.AllowedDepartmentIds?.Any() == true)
                {
                    dto.AllowedDepartments = dto.AllowedDepartmentIds
                        .Where(id => departmentDict.ContainsKey(id))
                        .Select(id => new DepartmentDTO
                        {
                            Id = departmentDict[id].Id,
                            Name = departmentDict[id].Name
                        })
                        .ToList();
                }

                if (dto.AllowedUserIds?.Any() == true)
                {
                    dto.AllowedUsers = dto.AllowedUserIds
                        .Where(id => userDict.ContainsKey(id))
                        .Select(id => new UserMiniDTO
                        {
                            Id = userDict[id].Id,
                            FullName = $"{userDict[id].FirstName} {userDict[id].LastName}"
                        })
                        .ToList();
                }
            }
        }

		public async Task<PagedResultDTO<DocumentDTO>> GetPagedByCategoryAsync(int page, int pageSize, int categoryId, int userId, int roleId, int departmentId)
		{
			var result = await _documentRepository.GetPagedByCategoryAsync(page, pageSize, categoryId, userId, roleId, departmentId);
			return new PagedResultDTO<DocumentDTO>
			{
				TotalCount = result.TotalCount,
				Page = result.Page,
				PageSize = result.PageSize,
				Items = _mapper.Map<List<DocumentDTO>>(result.Items)
			};
		}

		public async Task<DocumentCreatePreviewDTO> GetRevisionPreviewAsync(int documentId, int userId)
		{
			var document = await _documentRepository.GetDetailByIdAsync(documentId)
				?? throw new Exception("Document not found");   
            if(!document.IsLocked || document.LockedByUserId != userId)
			{
                throw new Exception("Document is not locked by the user");
			}
            return new DocumentCreatePreviewDTO
            {
				DocumentId = document.Id,
				IsRevision = true,

				DocumentCode = document.DocumentCode,
				CategoryId = document.CategoryId,
				CategoryName = document.Category?.Name ?? "-",
				CompanyId = document.CompanyId,
				CompanyName = document.Company?.Name ?? "-",

				VersionNumber = document.VersionNumber + 1,
				VersionNote = document.VersionNote,

				StatusId = document.StatusId,
				Status = "Revizyon",

				OwnerUserId = document.CreatedByUserId,
				OwnerName = document.CreatedByUser != null
			    ? document.CreatedByUser.FirstName + " " + document.CreatedByUser.LastName
			    : "-",

				CreatedAt = document.CreatedAt,

				IsCodeValid = true
			};
		}

	}
}
