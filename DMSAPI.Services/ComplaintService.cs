using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.ComplaintDTO;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IMapper _mapper;

        public ComplaintService(IComplaintRepository complaintRepository, IMapper mapper)
        {
            _complaintRepository = complaintRepository;
            _mapper = mapper;
        }

        public async Task<ComplaintDTO> CreateComplaintAsync(CreateComplaintDTO createComplaintDTO, int userId)
        {


            var entity = _mapper.Map<Complaint>(createComplaintDTO);

            if (entity.AssignedTo.HasValue && entity.AssignedTo.Value == 0)
                entity.AssignedTo = null;

            entity.CreatedBy = userId;
            entity.ReportedAt = DateTime.UtcNow;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.Status = "AÇIK";

            entity.NeedsCapa = entity.SeverityId >= 3 || entity.IsRepeat;

            entity.ComplaintNo = await GenerateComplaintNoAsync(entity.CompanyId);

            await _complaintRepository.AddAsync(entity);

            return _mapper.Map<ComplaintDTO>(entity);

        }

        public async Task DeleteComplaintAsync(int id, int userId)
        {
            var entity = await _complaintRepository.GetComplaintByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Complaint not found");
            entity.DeletedBy = userId;
            entity.DeletedAt = DateTime.UtcNow;
            entity.IsDeleted = true;
            await _complaintRepository.UpdateAsync(entity);
        }

        public async Task<PagedResultDTO<ComplaintDTO>> GetAllComplaintsAsync(int pageNumber, int pageSize)
        {
            var entity = await _complaintRepository.GetAllComplaintsAsync(pageNumber, pageSize);
            var dto = _mapper.Map<List<ComplaintDTO>>(entity.Items);
            return new PagedResultDTO<ComplaintDTO>
            {
                Items = dto,
                TotalCount = entity.TotalCount,
                Page = entity.Page,
                PageSize = entity.PageSize
            };
        }

        public async Task<ComplaintDTO?> GetComplaintByIdAsync(long id)
        {
            var entity = await _complaintRepository.GetComplaintByIdAsync(id);
            if (entity == null)
                return null;
            return _mapper.Map<ComplaintDTO>(entity);
        }

        public async Task UpdateClosedAsync(int id,int userId)
        {
            var entity = await _complaintRepository.GetComplaintByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Complaint not found");
            entity.IsClosed = true;
            entity.Status = "KAPALI";
            entity.ClosedBy = userId;
            entity.ClosedAt = DateTime.UtcNow;
            await _complaintRepository.UpdateAsync(entity);

        }

        public async Task<ComplaintDTO> UpdateComplaintAsync(int id, UpdateComplaintDTO updateComplaintDTO, int userId)
        {
            var entity = await _complaintRepository.GetComplaintByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Complaint not found");
            _mapper.Map(updateComplaintDTO, entity);
            if (entity.AssignedTo.HasValue && entity.AssignedTo.Value == 0)
                entity.AssignedTo = null;
            entity.UpdateBy = userId;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.Status = "GÜNCELLENDİ";
            await _complaintRepository.UpdateAsync(entity);
            return _mapper.Map<ComplaintDTO>(entity);
        }

        private Task<string> GenerateComplaintNoAsync(int companyId)
        {
            return Task.FromResult($"CMP-{companyId}-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}".Substring(0, 30));
        }
    }
}
