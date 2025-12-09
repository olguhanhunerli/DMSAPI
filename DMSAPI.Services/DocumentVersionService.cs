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
    public class DocumentVersionService : IDocumentVersionService
    {
        private readonly IDocumentVersionRepository _repository;
        private readonly IMapper _mapper;

        public DocumentVersionService(IDocumentVersionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task AddAsync(DocumentVersion version)
        {
            await _repository.AddAsync(version);
        }

        public async Task<List<DocumentVersionDTO>> GetByDocumentIdAsync(int documentId)
        {
            var list = await _repository.GetByDocumentIdAsync(documentId);
            return _mapper.Map<List<DocumentVersionDTO>>(list);
        }
    }
}
