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

        public async Task<ComplaintDTO> CreateComplaintAsync(CreateComplaintDTO createComplaintDTO, int userId, int companyId)
        {
            var entity = _mapper.Map<Complaint>(createComplaintDTO);

            entity.CompanyId = companyId;
            entity.CreatedBy = userId;
            entity.ReportedAt = DateTime.UtcNow;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.Status = "AÇIK";
            entity.IsClosed = false;
            entity.IsDeleted = false;
            entity.IsCapa = false;

            entity.NeedsCapa = entity.SeverityId >= 3 || entity.IsRepeat;
            entity.ComplaintNo = await GenerateComplaintNoAsync(entity.CompanyId);

            await _complaintRepository.AddAsync(entity);

            if (createComplaintDTO.Assignees != null && createComplaintDTO.Assignees.Count > 0)
            {
                var unique = createComplaintDTO.Assignees
                    .GroupBy(x => x.UserId)
                    .Select(g => g.First())
                    .ToList();

                if (!unique.Any(x => x.IsPrimary))
                    unique[0].IsPrimary = true;

                bool primarySet = false;
                foreach (var a in unique)
                {
                    if (a.IsPrimary)
                    {
                        if (!primarySet) primarySet = true;
                        else a.IsPrimary = false;
                    }
                }

                entity.Assignees = unique.Select(a => new ComplaintAssignee
                {
                    ComplaintId = entity.Id,
                    UserId = a.UserId,
                    IsPrimary = a.IsPrimary,
                    AssignedBy = userId,
                    AssignedAt = DateTime.UtcNow
                }).ToList();

                await _complaintRepository.UpdateAsync(entity);
            }

            return _mapper.Map<ComplaintDTO>(entity);

        }

        public async Task DeleteComplaintAsync(string complaintNo, int userId)
        {
            var entity = await _complaintRepository.GetByComplaintNoAsync(complaintNo);
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

        public async Task<ComplaintDTO?> GetComplaintByNoAsync(string complaintNo)
        {
            var entity = await _complaintRepository.GetByComplaintNoAsync(complaintNo);
            if (entity == null)
                return null;
            return _mapper.Map<ComplaintDTO>(entity);
        }

        public async Task<List<ComplaintForCapaSelectDTO>> GetComplaintsForCapaSelectAsync(int companyId, string? search, int take)
        {
            if (companyId <= 0)
                throw new Exception("companyId must be greater than 0");
            if (take <= 0) take = 50;
            if (take > 200) take = 200;
            return await _complaintRepository.GetForCapaSelectAsync(companyId, search, take);
        }

        public async Task<bool> IsCapaAsync(string complaintNo)
        {
            var entity = await _complaintRepository.GetByComplaintNoAsync(complaintNo);
            if(entity == null) return false;
            entity.IsCapa = true;
            await _complaintRepository.UpdateAsync(entity);
            return true;
        }

        public async Task UpdateClosedAsync(string complaintNo,int userId)
        {
            var entity = await _complaintRepository.GetByComplaintNoAsync(complaintNo);
            if (entity == null)
                throw new KeyNotFoundException("Complaint not found");
            entity.IsClosed = true;
            entity.Status = "KAPALI";
            entity.ClosedBy = userId;
            entity.ClosedAt = DateTime.UtcNow;
            entity.IsDeleted = false;
            entity.IsCapa = false;
            await _complaintRepository.UpdateAsync(entity);

        }

        public async Task<ComplaintDTO> UpdateComplaintByNoAsync(string complaintNo, UpdateComplaintDTO updateComplaintDTO, int userId, int companyId)
        {
			var entity = await _complaintRepository.GetByComplaintNoAsync(complaintNo);
			if (entity == null)
				throw new KeyNotFoundException("Complaint not found");

			updateComplaintDTO.CompanyId = companyId;

			_mapper.Map(updateComplaintDTO, entity);

			entity.UpdateBy = userId;
			entity.UpdatedAt = DateTime.UtcNow;
			entity.Status = "GÜNCELLENDİ";
            entity.IsClosed = false;

            entity.NeedsCapa = entity.SeverityId >= 3 || entity.IsRepeat;

			await _complaintRepository.UpdateAsync(entity);

			return _mapper.Map<ComplaintDTO>(entity);
		}

        private Task<string> GenerateComplaintNoAsync(int companyId)
        {
            return Task.FromResult($"CMP-{companyId}-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}".Substring(0, 30));
        }
    }
}
