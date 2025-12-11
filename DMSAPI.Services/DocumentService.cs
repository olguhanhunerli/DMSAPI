using AutoMapper;
using DMSAPI.Business.Repositories;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
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
        private readonly ICategoryServices _categoryServices;
        private readonly IDocumentAttachmentRepository _documentAttachmentRepository;
        private readonly IDocumentApprovalRepository _documentApprovalRepository;


        public DocumentService(
            IDocumentRepository documentRepository,
            IMapper mapper,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IHostEnvironment env,
            ICategoryServices categoryServices,
            IDocumentAttachmentRepository documentAttachmentRepository,
            IDocumentApprovalRepository documentApprovalRepository)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _env = env;
            _categoryServices = categoryServices;
            _documentAttachmentRepository = documentAttachmentRepository;
            _documentApprovalRepository = documentApprovalRepository;
        }

        public async Task<DocumentCreateResponseDTO> CreateDocumentAsync(DocumentCreateDTO dto, int userId)
        {
            using var transaction = await _documentRepository.BeginTransactionAsync();
            try
            {
                var user = await _userRepository
                                 .GetAllUserAsync()
                                 .ContinueWith(t => t.Result.FirstOrDefault(x => x.Id == userId))
                                 ?? throw new Exception("User not found");

                if (user.Company == null)
                    throw new Exception("User.Company is NULL");

                var category = await _categoryRepository.GetByIdAsync(dto.CategoryId)
                    ?? throw new Exception("Category not found");

                int companyId = user.CompanyId;

                int number = await _documentRepository
                    .GetNextDocumentNumberAsync(companyId, dto.CategoryId);

                string code = $"{user.Company.CompanyCode}-{category.Code}-{number:D3}";

                if (await _documentRepository.DocumentCodeExistingAsync(code))
                    throw new Exception("Document code already exists");

                var document = _mapper.Map<Document>(dto);

                document.CompanyId = companyId;
                document.DocumentCode = code;
                document.StatusId = 1;
                document.CreatedByUserId = userId;
                document.CreatedAt = DateTime.UtcNow;
                document.IsDeleted = false;

                await _documentRepository.AddAsync(document);

                if (dto.ApproverUserIds != null && dto.ApproverUserIds.Any())
                {
                    int level = 1;

                    foreach (var approverId in dto.ApproverUserIds)
                    {
                        var approval = new DocumentApproval
                        {
                            DocumentId = document.Id,
                            UserId = approverId,
                            ApprovalLevel = level,
                            IsApproved = false,
                            IsRejected = false,
                            CreatedAt = DateTime.UtcNow
                        };

                        await _documentApprovalRepository.AddAsync(approval);
                        level++;
                    }

                }

                if (dto.Files != null && dto.Files.Any())
                {
                    var baseFolder = Path.Combine(
                        _env.ContentRootPath,
                        "uploads",
                        "documents",
                        document.DocumentCode,
                        "attachments"
                    );

                    if (!Directory.Exists(baseFolder))
                        Directory.CreateDirectory(baseFolder);

                    foreach (var file in dto.Files)
                    {
                        var cleanName = Path.GetFileName(file.FileName);
                        var fullPath = Path.Combine(baseFolder, cleanName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var attachment = new DocumentAttachment
                        {
                            DocumentId = document.Id,
                            FileName = cleanName,
                            OriginalFileName = file.FileName,
                            FileSize = file.Length,
                            FileType = Path.GetExtension(file.FileName),
                            FilePath = $"uploads/documents/{document.DocumentCode}/attachments/{cleanName}",
                            UploadedAt = DateTime.UtcNow,
                            UploadedByUserId = userId,
                            IsMainFile = false,
                            IsDeleted = false
                        };

                        await _documentAttachmentRepository.AddAsync(attachment);
                    }

                }

                await transaction.CommitAsync();

                return _mapper.Map<DocumentCreateResponseDTO>(document);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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
