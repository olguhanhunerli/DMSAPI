using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.DocumentAttachmentDTO.cs;
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
	public class DocumentAttachmentService : IDocumentAttachmentService
    {
        private readonly IDocumentAttachmentRepository _documentAttachmentRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _env;
        public DocumentAttachmentService(IDocumentAttachmentRepository documentAttachmentRepository, IMapper mapper, IHostEnvironment env, IDocumentRepository documentRepository)
        {
            _documentAttachmentRepository = documentAttachmentRepository;
            _mapper = mapper;
            _env = env;
            _documentRepository = documentRepository;
        }

        public async Task AddAsync(DocumentAttachment attachment)
        {
             await _documentAttachmentRepository.AddAsync(attachment);    
        }

		public async Task<DownloadFileResultDTO> DownloadAttachmentAsync(int attachmentId)
		{
			var attachment = await _documentAttachmentRepository.GetByIdAsync(attachmentId)
			?? throw new Exception("Attachment not found");

			if (string.IsNullOrWhiteSpace(attachment.FilePath))
				throw new Exception("Attachment file path not found");

			if (!File.Exists(attachment.FilePath))
				throw new Exception("Attachment file does not exist");

			var bytes = await File.ReadAllBytesAsync(attachment.FilePath);

			return new DownloadFileResultDTO
			{
				FileBytes = bytes,
				OriginalFileName = attachment.OriginalFileName,
				ContentType = GetContentType(attachment.OriginalFileName)
			};
		}

		public async Task<List<DocumentAttachmentDTO>> GetByDocumentIdAsync(int documentId)
        {
            var list = await _documentAttachmentRepository.GetByDocumentIdAsync(documentId);
            return _mapper.Map<List<DocumentAttachmentDTO>>(list);
        }

        public async Task UploadMultipleAsync(CreateDocumentAttachmentDTO dto, int userId)
        {
            var document = await _documentRepository.GetByIdAsync(dto.DocumentId);
            if (document == null)
            {
                throw new ArgumentException("Document Not Found");
            }
            var documentFolderPath = Path.Combine(_env.ContentRootPath, "uploads", "documents", document.DocumentCode);
            if (!Directory.Exists(documentFolderPath))
            {
                throw new Exception("Document Folder Not Found");
            }
            foreach (var file in dto.Files)
            {
                var cleanFileName = Path.GetFileName(file.FileName);
                var fullPath = Path.Combine(documentFolderPath, cleanFileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var attachment = new DocumentAttachment
                {
                    DocumentId = dto.DocumentId,

                    FileName = cleanFileName,
                    OriginalFileName = file.FileName,
                    FileSize = file.Length,
                    FileType = Path.GetExtension(file.FileName),
                    FilePath = $"/uploads/documents/{document.DocumentCode}/{cleanFileName}",

                    UploadedAt = DateTime.UtcNow,
                    UploadedByUserId = userId,

                    IsMainFile = dto.IsMainFile,
                    IsDeleted = false
                };
                await _documentAttachmentRepository.AddAsync(attachment);
            }
        }
		private static string GetContentType(string fileName)
		{
			var ext = Path.GetExtension(fileName).ToLowerInvariant();

			return ext switch
			{
				".pdf" => "application/pdf",
				".jpg" or ".jpeg" => "image/jpeg",
				".png" => "image/png",
				".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
				".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
				_ => "application/octet-stream"
			};
		}
	}
}
