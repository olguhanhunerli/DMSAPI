using AutoMapper;
using DMSAPI.Business.Repositories;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.CapaEvidenceFiles;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
	public class CapaEvidenceService : ICapaEvidenceService
    {
        private readonly ICapaEvidenceRepository _repository;
		private readonly IMapper _mapper;
		private readonly ICAPARepository _capaRepository;
		private readonly IHostEnvironment _env;
		private static readonly SemaphoreSlim _loLock = new(1, 1);
		public CapaEvidenceService(ICapaEvidenceRepository repository, IMapper mapper, IHostEnvironment env, ICAPARepository capaRepository)
		{
			_repository = repository;
			_mapper = mapper;
			_env = env;
			_capaRepository = capaRepository;
		}
		public async Task<CapaEvidenceFiles> CreateFiles(string capaNo, CreateCapaEvidenceFilesDTO dto, int userId)
		{
			var capa = await _capaRepository.GetCAPAByCapaNoAsync(capaNo);
			if (capa == null)
				throw new Exception("CAPA not found");

			var file = dto.File;
			if (file == null || file.Length == 0)
				throw new Exception("File is empty");

			var folder = Path.Combine(
				_env.ContentRootPath,
				"files",
				"capa_evidence",
				capaNo
			);
			Directory.CreateDirectory(folder);

			var cleanName = Path.GetFileName(file.FileName);
			var fullPath = Path.Combine(folder, cleanName);

			using (var stream = new FileStream(fullPath, FileMode.Create))
				await file.CopyToAsync(stream);

			var capaFolder = Path.GetFileName(capaNo); 
			var relPath = $"/files/capa_evidence/{capaFolder}/{cleanName}";
			var ext = Path.GetExtension(file.FileName);         
			var mime = file.ContentType;
			var entity = new CapaEvidenceFiles
			{
				CapaNo = capa.CapaNo,
				FileName = file.FileName,          
				FilePath = relPath,                
				FileType = ext,

				UploadedAt = DateTime.UtcNow,
				UploadedBy = userId
			};

			await _repository.AddAsync(entity);
			return entity;

		}

		public async Task<(string FullPath, string FileName, string ContentType)> DownloadFileAsync(string capaNo, long fileId)
		{
			var entity = await _repository.GetByIdLongAsync(fileId);
			if (entity == null)
				throw new Exception("Dosya bulunamadı");

			if (!string.Equals(entity.CapaNo, capaNo, StringComparison.OrdinalIgnoreCase))
				throw new Exception("Dosya bu CAPA'ya ait değil");

			var relative = entity.FilePath.TrimStart('/');
			var fullPath = Path.Combine(_env.ContentRootPath, relative);

			if (!System.IO.File.Exists(fullPath))
				throw new Exception("Dosya disk üzerinde bulunamadı");

			var provider = new FileExtensionContentTypeProvider();
			if (!provider.TryGetContentType(entity.FileName, out var contentType))
				contentType = "application/octet-stream";

			return (fullPath, entity.FileName, contentType);
		}
	}
}
