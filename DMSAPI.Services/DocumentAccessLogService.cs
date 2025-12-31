using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
    public class DocumentAccessLogService : IDocumentAccessLogService
    {
        private readonly IDocumentAccessLogRepository _repository;
        private readonly IMapper _mapper;

        public DocumentAccessLogService(IDocumentAccessLogRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task AddAsync(DocumentAccessLog log)
        {
           await _repository.AddAsync(log);
        }

        public async Task<PagedResultDTO<DocumentAccessLogDTO>> GetAllAsync(DocumentAccessLogFilterDTO queryDto)
        {
            var query = await _repository.GetQueryableAsync(queryDto);
            var totalCount = query.Count();
            var logs = query
                .Skip((queryDto.Page - 1) * queryDto.PageSize)
                .Take(queryDto.PageSize)
                .ToList();
            var items = await query
                .Skip((queryDto.Page - 1) * queryDto.PageSize)
                .Take(queryDto.PageSize)
                .Select(x => new DocumentAccessLogDTO
                {
                    Id = x.Id,
                    DocumentId = x.DocumentId,
                    DocumentCode = x.Document.DocumentCode,
                    UserId = x.UserId,
                    UserName = x.User.FirstName + " " + x.User.LastName,
                    AccessType = x.AccessType,
                    IpAddress = x.IpAddress,
                    AccessAt = x.AccessAt
                })
                .ToListAsync();

            return new PagedResultDTO<DocumentAccessLogDTO>
            {
                Items = items,
                Page = queryDto.Page,
                PageSize = queryDto.PageSize,
                TotalCount = totalCount
            };
        }
    }
}
