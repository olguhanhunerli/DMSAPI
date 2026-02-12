using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.CapaActionFileDTO;
using DMSAPI.Entities.DTOs.CAPADTO;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
	public class CAPAActionsService : ICAPAActionsService
    {
        private readonly ICAPAActionRepository _actionRepository;
        private readonly ICAPARepository _capaRepository;
        private readonly IMapper _mapper;
        private readonly IActionFilesRepository _actionFilesRepository;
		private readonly IHostEnvironment _env;

		public CAPAActionsService(ICAPAActionRepository actionRepository, ICAPARepository capaRepository, IMapper mapper, IActionFilesRepository actionFilesRepository, IHostEnvironment env)
		{
			_actionRepository = actionRepository;
			_capaRepository = capaRepository;
			_mapper = mapper;
			_actionFilesRepository = actionFilesRepository;
			_env = env;
		}

		public async Task<CAPAActionDTO> CreateActionAsync(string capaNo, CreateCAPAActionDTO action, int userId, int companyId)
        {
            var capa = await _capaRepository.GetCAPAByCapaNoAsync(capaNo);
            if (capa == null) 
            {
                throw new Exception("CAPA Kaydı Bulunamadı");
            }
            if (capa.CompanyId != companyId)
            {
                throw new Exception("Yetkisiz İşlem");
            }
            if (capa.IsClosed) 
            {
                throw new Exception("Kapalı CAPA Aksiyon Eklenemez");
            }
            if (string.IsNullOrWhiteSpace(action.Description))
                throw new Exception("Aksiyon açıklaması zorunlu");

            var actions = new CAPAACTION
            {
                CapaNo = capaNo,
                ActionType = action.ActionType,
                Description = action.Description,
                OwnerId = action.OwnerId,
                DueDate = action.DueDate,
                EvidenceRequired = action.EvidenceRequired,
                Status = "BEKLİYOR",
                CreatedAt = DateTime.UtcNow
            };
            await _actionRepository.AddAsync(actions);

            if (capa.Status == "BEKLIYOR")
            {
                capa.Status = "DEVAM EDIYOR";
                capa.UpdatedAt = DateTime.UtcNow;
                await _capaRepository.UpdateAsync(capa);
            }
            return _mapper.Map<CAPAActionDTO>(actions);
        }

		public async Task<(string FullPath, string FileName, string ContentType)> DownloadActionFileAsync(long actionId, long fileId)
		{
			var file = await _actionFilesRepository.GetByLongIdAsync(fileId);
			if (file == null || file.ActionId != actionId)
				throw new Exception("Dosya bulunamadı");

			var action = await _actionRepository.GetByIdLongAsync(actionId);
			if (action == null)
				throw new Exception("Aksiyon bulunamadı");

			var capa = await _capaRepository.GetCAPAByCapaNoAsync(action.CapaNo);
			var relativePath = file.FilePath.TrimStart('/');
			var fullPath = Path.Combine(_env.ContentRootPath, relativePath);

			if (!System.IO.File.Exists(fullPath))
				throw new Exception("Dosya disk üzerinde bulunamadı");

			var contentType = file.FileType ?? "application/octet-stream";

			return (fullPath, file.FileName, contentType);
		}

		public async Task<List<CAPAActionDTO>> GetByCapaNoAsync(string capaNo, int companyId)
        {
            var capa = await _capaRepository.GetCAPAByCapaNoAsync(capaNo);
            if (capa == null)
                throw new Exception("CAPA Bulunamadı");
            var actions = await _actionRepository.GetByCapaNoAsync(capaNo);
            return _mapper.Map<List<CAPAActionDTO>>(actions);
        }

        public async Task<CAPAActionDTO> UpdateActionAsync(long actionId, UpdateCAPAActionDTO dto, int userId, int companyId)
        {
            var entity = await _actionRepository.GetByIdLongAsync(actionId);
            if (entity == null)
                throw new Exception("Aksiyon bulunamadı");

            var capa = await _capaRepository.GetCAPAByCapaNoAsync(entity.CapaNo);
            if (capa == null)
                throw new Exception("CAPA bulunamadı");

            if (capa.CompanyId != companyId)
                throw new Exception("Yetkisiz işlem");

            if (capa.IsClosed)
                throw new Exception("Kapalı CAPA'da aksiyon güncellenemez");
            if (dto.Description != null) entity.Description = dto.Description;
            if (dto.OwnerId.HasValue) entity.OwnerId = dto.OwnerId.Value;
            if (dto.DueDate.HasValue) entity.DueDate = dto.DueDate.Value;
            if (dto.EvidenceRequired.HasValue) entity.EvidenceRequired = dto.EvidenceRequired.Value;
            if (dto.CompletionNote != null) entity.CompletionNote = dto.CompletionNote;
			if (!string.IsNullOrWhiteSpace(dto.Status))
			{
				var isCompleting = dto.Status.ToUpperInvariant().Contains("TAMAM");

				if (isCompleting && entity.EvidenceRequired)
				{
					var hasEvidence = await _actionFilesRepository.AnyByActionIdAsync(actionId);
					if (!hasEvidence)
						throw new Exception("Evidence zorunlu. Dosya yüklemeden tamamlayamazsın.");
				}

				entity.Status = dto.Status;

				if (isCompleting)
				{
					entity.CompletedAt = DateTime.UtcNow;
					entity.CompletedBy = userId;
				}
				else
				{
					entity.CompletedAt = null;
					entity.CompletedBy = null;
				}
			}
			entity.UpdatedAt = DateTime.UtcNow;

            await _actionRepository.UpdateAsync(entity);

            return _mapper.Map<CAPAActionDTO>(entity);
        }

		public async Task<bool> UploadActionFileAsync(long actionId, CreateActionFilesDTO dto, int userId)
		{
			var file = dto?.File;
			if (file == null || file.Length == 0)
				throw new Exception("File is empty");

			var action = await _actionRepository.GetByIdLongAsync(actionId);
			if (action == null)
				throw new Exception("Aksiyon bulunamadı");

			var capa = await _capaRepository.GetCAPAByCapaNoAsync(action.CapaNo);
			if (capa == null)
				throw new Exception("CAPA bulunamadı");


			if (capa.IsClosed)
				throw new Exception("Kapalı CAPA'ya dosya eklenemez");

			var capaNo = capa.CapaNo;
			var folder = Path.Combine(
				_env.ContentRootPath,
				"files",
				"capa_evidence",
				capaNo,
				actionId.ToString()
			);
			Directory.CreateDirectory(folder);

			var cleanName = Path.GetFileName(file.FileName);
			var ext = Path.GetExtension(cleanName);
			if (string.IsNullOrWhiteSpace(ext)) ext = ".bin";

			var storedName = $"{cleanName}{ext}";
			var fullPath = Path.Combine(folder, storedName);

			await using (var stream = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
			{
				await file.CopyToAsync(stream);
			}

			var relPath = $"/files/capa_evidence/{capaNo}/{actionId}/{storedName}";

			var entity = new ActionFile
			{
				ActionId = actionId,
				FileName = cleanName,
				FilePath = relPath,         
				FileType = file.ContentType, 
				UploadedAt = DateTime.UtcNow,
				UploadedBy = userId
			};

			await _actionFilesRepository.AddAsync(entity);
			return true;
		}
	}
}
