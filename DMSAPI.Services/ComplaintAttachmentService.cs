using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.ComplaintAttachment;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
	public class ComplaintAttachmentService : IComplaintAttachmentService
    {
        private readonly IComplaintAttachmentRepository _complaintAttachmentRepository;
		private readonly IMapper _mapper;
		private readonly IHostEnvironment _hostEnvironment;

		public ComplaintAttachmentService(IComplaintAttachmentRepository complaintAttachmentRepository, IMapper mapper, IHostEnvironment hostEnvironment)
		{
			_complaintAttachmentRepository = complaintAttachmentRepository;
			_mapper = mapper;
			_hostEnvironment = hostEnvironment;
		}

		public async Task<bool> DeleteAsync(long id, int userId)
		{
			var attachments = await _complaintAttachmentRepository.GetByIdAsync(id);
			if (attachments == null)
				return false;
			attachments.IsDeleted = true;
			attachments.DeletedAt = DateTime.UtcNow;
			attachments.DeletedBy = userId;
			await _complaintAttachmentRepository.UpdateAsync(attachments);
			return true;
		}

		public async Task<(Stream Stream, string ContentType, string DownloadFileName)> DownloadAsync(long id)
		{
			var attachment = await _complaintAttachmentRepository.GetByIdAsync(id)
	   ?? throw new Exception("Attachment not found");

			if (attachment.IsDeleted)
				throw new Exception("Attachment is deleted");

			if (string.IsNullOrWhiteSpace(attachment.StorageKey))
				throw new Exception("Attachment storage key is invalid");
			var filePath = ToPhysicalPath(attachment.StorageKey);

			if (!File.Exists(filePath))
				throw new Exception("Attachment file not found");

			var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

			var contentType = !string.IsNullOrWhiteSpace(attachment.ContentType)
				? attachment.ContentType
				: GetContentType(attachment.OriginalFileName);

			return (stream, contentType, attachment.OriginalFileName);
		}

		public async Task<List<ComplaintAttachmentDTO>> GetByComplaintNoAsync(string complaintNo)
		{
			complaintNo = complaintNo.Trim();
			var attachments = await _complaintAttachmentRepository.GetByComplaintNoAsync(complaintNo);
			return _mapper.Map<List<ComplaintAttachmentDTO>>(attachments);
		}

		public async Task<ComplaintAttachmentDTO> UploadAsync(string complaintNo, IFormFile file, int userId)
		{
			complaintNo = complaintNo.Trim();
			if(file == null || file.Length == 0)
				throw new Exception("File is empty");
			var folder = Path.Combine(_hostEnvironment.ContentRootPath,"files","complaints", complaintNo);
			Directory.CreateDirectory(folder);
			var originalFileName = Path.GetFileName(file.FileName);
			var ext = Path.GetExtension(originalFileName);
			var storedFileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{ext}";
			var fullPath = Path.Combine(folder, storedFileName);
			var storageKey = Path.Combine("files", "complaints", complaintNo, storedFileName);
			using (var stream = new FileStream(fullPath, FileMode.Create))
			{
				file.CopyTo(stream);
			}
			try
			{
				var attachments = new ComplaintAttachment
				{
					ComplaintNo = complaintNo,
					OriginalFileName = originalFileName,
					ContentType = file.ContentType,
					FileSize = file.Length,
					StorageKey = storageKey,
					UploadedAt = DateTime.UtcNow,
					UploadedBy = userId,
					IsDeleted = false
				};
				await _complaintAttachmentRepository.AddAsync(attachments);
				return _mapper.Map<ComplaintAttachmentDTO>(attachments);
			}
			catch
			{
				if(File.Exists(fullPath))
				{
					File.Delete(fullPath);
				}
				throw;
			}
		}
		private string ToPhysicalPath(string storageKeyOrPath)
		{
			var relative = storageKeyOrPath.Trim();

			if (Path.IsPathRooted(relative))
				return relative;

			relative = relative.TrimStart('/');

			relative = relative.Replace("/", Path.DirectorySeparatorChar.ToString());

			return Path.Combine(_hostEnvironment.ContentRootPath, relative);
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
