using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
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
    }
}
