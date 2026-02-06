using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.CAPADTO;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.InstrumentCalibrationDTOs;
using DMSAPI.Entities.DTOs.RootCouseMethodDTO;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
    public class CAPAServices : ICAPAServices
    {
        private readonly ICAPARepository _capaRepository;
        private readonly IMapper _mapper;

        public CAPAServices(ICAPARepository capaRepository, IMapper mapper)
        {
            _capaRepository = capaRepository;
            _mapper = mapper;
        }

        public async Task<CAPADTO> ClosedCapaAsync(string capaNo, ClosedCAPADTO dto, int userId)
        {
            if (string.IsNullOrWhiteSpace(capaNo))
                throw new Exception("CapaNo boş olamaz");

            var entity = await _capaRepository.GetByCapaNoForCloseAsync(capaNo);
            if (entity == null)
                throw new Exception("CAPA Bulunamadı");

            if (entity.IsClosed == true)
                throw new Exception("CAPA Zaten Kapalı");

            if (entity.Actions == null || entity.Actions.Count == 0)
                throw new Exception("En Az 1 Aksiyon Girilmeli");

            var notDone = entity.Actions
                .Where(a => string.IsNullOrWhiteSpace(a.Status) ||
                            !a.Status.Trim().ToUpper().Contains("TAMAM"))
                .Select(a => a.Id)
                .ToList();

            if (notDone.Any())
                throw new Exception($"Kapatılamaz. Tamamlanmamış aksiyonlar var: {string.Join(", ", notDone)}");

            if (dto.EffectivenessCheck != null) entity.EffectivenessCheck = dto.EffectivenessCheck;
            if (dto.EffectivenessResult != null) entity.EffectivenessResult = dto.EffectivenessResult;

            entity.EffectivenessCheckedBy = dto.EffectivenessCheckedBy ?? userId;
            entity.EffectivenessCheckedAt = dto.EffectivenessCheckedAt ?? DateTime.UtcNow;

            if (dto.ClosureEvidence != null)
                entity.ClosureEvidence = dto.ClosureEvidence;

            entity.IsClosed = true;
            entity.ClosedAt = DateTime.UtcNow;
            entity.Status = "CLOSED";
            entity.UpdatedAt = DateTime.UtcNow;

            await _capaRepository.UpdateAsync(entity);

            var full = await _capaRepository.GetCAPAByCapaNoAsync(capaNo);
            if (full != null)
                throw new Exception("CAPA bulunamadı");

            return _mapper.Map<CAPADTO>(full);
        }

        public async Task<CAPADTO> CreateCapaAsync(CreateCAPADTO createCAPADTO, int userId, int companyId)
        {
            var rootCauseMethodOk = await _capaRepository.RootCauseMethodExistsAsync(createCAPADTO.RootCauseMethodId);
            if (!rootCauseMethodOk)
                throw new Exception("Geçersiz Method");

            var companyCode = await _capaRepository.GetCompanyCodeAsync(companyId);
            if (string.IsNullOrWhiteSpace(companyCode))
                throw new Exception("CompanyCode tanımlı değil.");

            companyCode = companyCode.Trim().Replace(" ", "").ToUpperInvariant();

            var entity = _mapper.Map<CAPA>(createCAPADTO);
            entity.CompanyId = companyId;
            entity.OwnerId = userId;                 
            entity.CreatedAt = DateTime.UtcNow;    
            entity.IsClosed = false;
            entity.OpenedAt = DateTime.UtcNow;
            entity.UpdatedAt = null;
            entity.CapaNo = $"TMP-{Guid.NewGuid():N}";
            await _capaRepository.AddAsync(entity);
            await _capaRepository.SaveChangesAsync();

            entity.CapaNo = $"{companyCode}-CAPA-{DateTime.UtcNow:yyyy}-{entity.Id:D6}";

            await _capaRepository.SaveChangesAsync();

            return _mapper.Map<CAPADTO>(entity);
        }

        public async Task<CAPADTO> GetCAPAByCapaNoAsync(string capaNo)
        {
            var result = await _capaRepository.GetCAPAByCapaNoAsync(capaNo);
            if (result == null)
            {
                throw new Exception("Bulunamadı");
            }
            return _mapper.Map<CAPADTO>(result);
        }

        public async Task<PagedResultDTO<CAPADTO>> GetCapaPagedResult(int page, int pageSize)
        {
            var result = await _capaRepository.GetAllCAPAAsync(page, pageSize);
            if (result == null)
            {
                throw new Exception("Bulunamadı");
            }
            var mappedItems = _mapper.Map<List<CAPADTO>>(result.Items);
            return new PagedResultDTO<CAPADTO>
            {
                Items = mappedItems,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };

        }

        public async Task<CapaCreateFormInitDTO> GetCreateFormInitAsync(string complaintNo, int userId, int companyId)
        {
            if (string.IsNullOrWhiteSpace(complaintNo))
                throw new Exception("Şikayet numarası boş olamaz");

            var complaint = await _capaRepository.GetComplaintDtoByNoAsync(complaintNo);
            if (complaint == null)
                throw new Exception("Şikayet bulunamadı");


            //var alreadyHasCapa = await _capaRepository.ComplaintExistsAsync(complaintNo);
            //if (alreadyHasCapa)
            //    throw new Exception("Bu şikayet için zaten bir CAPA mevcut");

            var customer = await _capaRepository.GetCustomerMiniByIdAsync(complaint.CustomerId);
            if (customer == null)
                throw new Exception("Müşteri bulunamadı");

            var rootCauseMethods = await _capaRepository.GetRootCauseMethodLookupsAsync();

            var companyName = complaint.CompanyName;

            var ownerName = await _capaRepository.GetUserFullNameByIdAsync(userId);

            var defaults = new CreateCapaDefaultsDTO
            {
                ComplaintNo = complaintNo,
                CompanyId = companyId,
                CompanyName = companyName,          
                OwnerId = userId,
                OwnerName = ownerName,   
                DueDate = DateTime.UtcNow.Date.AddDays(30),
                Status = "AÇIK"
            };

            var lookups = new CapaCreateLookupsDTO
            {
                RootCauseMethods = rootCauseMethods,
            };

            return new CapaCreateFormInitDTO
            {
                Customer = customer,
                Complaint = complaint,
                Defaults = defaults,
                Lookups = lookups
            };
        }

        public async Task<List<RootCauseMethod>> GetRootCouseMethodAsync()
        {
            var entity = await _capaRepository.GetRootCouseMethodAsync();

            return entity.Select(x => new RootCauseMethod
            {
                Id = x.Id,
                Code = x.Code,
                NameTr = x.NameTr
            }).ToList();
        }

        public async Task<CAPADTO> UpdateCapaAsync(string capaNo, UpdateCAPADTO dto, int userId, int companyId)
        {
            if (string.IsNullOrWhiteSpace(capaNo))
                throw new Exception("CapaNo boş olamaz");

            var entity = await _capaRepository.GetByCapaNoForUpdateAsync(capaNo);
            if (entity == null)
                throw new Exception("CAPA bulunamadı");

            if (entity.CompanyId != companyId)
                throw new Exception("Yetkisiz işlem");

            if (entity.IsClosed == true)
                throw new Exception("Kapalı CAPA güncellenemez");

            if (dto.RootCauseMethodId.HasValue)
            {
                var ok = await _capaRepository.RootCauseMethodExistsAsync(dto.RootCauseMethodId.Value);
                if (!ok) throw new Exception("Geçersiz Method");
                entity.RootCauseMethodId = dto.RootCauseMethodId.Value;
            }

            if (dto.Nonconformity != null) entity.Nonconformity = dto.Nonconformity;
            if (dto.RootCause != null) entity.RootCause = dto.RootCause;
            if (dto.CorrectiveAction != null) entity.CorrectiveAction = dto.CorrectiveAction;
            if (dto.DueDate.HasValue) entity.DueDate = dto.DueDate;
            if (dto.OwnerId.HasValue) entity.OwnerId = dto.OwnerId.Value;
            if (dto.Status != null) entity.Status = dto.Status;

            if (dto.EffectivenessCheck != null) entity.EffectivenessCheck = dto.EffectivenessCheck;
            if (dto.EffectivenessCheckedBy.HasValue) entity.EffectivenessCheckedBy = dto.EffectivenessCheckedBy.Value;
            if (dto.EffectivenessCheckedAt.HasValue) entity.EffectivenessCheckedAt = dto.EffectivenessCheckedAt;
            if (dto.EffectivenessResult != null) entity.EffectivenessResult = dto.EffectivenessResult;

            entity.UpdatedAt = DateTime.UtcNow;

            await _capaRepository.UpdateAsync(entity);

            var full = await _capaRepository.GetCAPAByCapaNoAsync(capaNo);
            if (full == null)
                throw new Exception("CAPA bulunamadı");

            return _mapper.Map<CAPADTO>(full);
        }
    }
}
