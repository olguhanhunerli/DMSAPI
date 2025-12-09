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
    public class DocumentAttachmentService : IDocumentAttachmentService
    {
        private readonly IDocumentAttachmentRepository _documentAttachmentRepository;
        private readonly IMapper _mapper;
        public DocumentAttachmentService(IDocumentAttachmentRepository documentAttachmentRepository, IMapper mapper)
        {
            _documentAttachmentRepository = documentAttachmentRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(DocumentAttachment attachment)
        {
             await _documentAttachmentRepository.AddAsync(attachment);    
        }

        public async Task<List<DocumentAttachmentDTO>> GetByDocumentIdAsync(int documentId)
        {
            var list = await _documentAttachmentRepository.GetByDocumentIdAsync(documentId);
            return _mapper.Map<List<DocumentAttachmentDTO>>(list);
        }
    }
}
