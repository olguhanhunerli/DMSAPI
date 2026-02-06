using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.CAPADTO;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
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

        public CAPAActionsService(ICAPAActionRepository actionRepository, ICAPARepository capaRepository, IMapper mapper)
        {
            _actionRepository = actionRepository;
            _capaRepository = capaRepository;
            _mapper = mapper;
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

        public async Task<List<CAPAActionDTO>> GetByCapaNoAsync(string capaNo, int companyId)
        {
            var capa = _capaRepository.GetCAPAByCapaNoAsync(capaNo);
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
                entity.Status = dto.Status;

                if (dto.Status.ToUpperInvariant().Contains("TAMAM"))
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
    }
}
