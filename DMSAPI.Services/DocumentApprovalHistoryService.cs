using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
    public class DocumentApprovalHistoryService : IDocumentApprovalHistoryService
    {
        private readonly IDocumentApprovalHistoryRepository _repository;
        private readonly IMapper _mapper;

        public DocumentApprovalHistoryService(IDocumentApprovalHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task AddAsync(DocumentApprovalHistory history)
        {
             await _repository.AddAsync(history);
        }

        public async Task<List<DocumentApprovalHistoryDTO>> GetByDocumentIdAsync(int documentId)
        {
            var list = await _repository.GetByDocumentIdAsync(documentId);
            return _mapper.Map<List<DocumentApprovalHistoryDTO>>(list);
        }
    }
}
