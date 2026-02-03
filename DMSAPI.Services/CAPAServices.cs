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

        public async Task<CAPADTO> CreateCapaAsync(CreateCAPADTO createCAPADTO, int userId, int companyId)
        {
            var rootCauseMethodOk = await _capaRepository.RootCauseMethodExistsAsync(createCAPADTO.RootCauseMethodId);
            if (!rootCauseMethodOk)
                throw new Exception("Geçersiz Method");

            if (!string.IsNullOrWhiteSpace(createCAPADTO.ComplaintNo))
            {
                var complaintOk = await _capaRepository.ComplaintExistsAsync(createCAPADTO.ComplaintNo);
                if (!complaintOk)
                    throw new Exception("Geçersiz Şikayet");
            }

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
            var complaint = await _capaRepository.GetComplaintDtoByNoAsync(complaintNo);
            if (complaint == null)
                throw new Exception("Şikayet bulunamadı");

            var customer = await _capaRepository.GetCustomerMiniByIdAsync(complaint.CustomerId);
            if (customer == null)
                throw new Exception("Müşteri bulunamadı");

            var rootCauseMethods = await _capaRepository.GetRootCauseMethodLookupsAsync();

            var defaults = new CreateCapaDefaultsDTO
            {
                ComplaintNo = complaintNo,
                CompanyId = companyId,
                OwnerId = userId,
                DueDate = DateTime.UtcNow.Date.AddDays(30), 
                Status = "OPEN"
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
    }
}
