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

		public async Task CreateVersionFromRevisionAsync(DocumentRevision revision, string filePath, int userId)
		{
            Console.WriteLine($"REV: id={revision.Id} code='{revision.DocumentCode}' newVer={revision.NewVersionNumber}");
			var versions = await _repository.GetByDocumentIdAsync(revision.DocumentId);
          		
            var newVersion = new DocumentVersion
			{
				DocumentId = revision.DocumentId,
				VersionNumber = revision.NewVersionNumber,
				FilePath = filePath,
				CreatedByUserId = userId,
				CreatedAt = DateTime.UtcNow,
				VersionNote = revision.RevisionNote,
                DocumentCode = revision.DocumentCode!,
			};

            await _repository.AddAsync(newVersion);
		}

		public async Task<List<DocumentVersionDTO>> GetByDocumentIdAsync(int documentId)
        {
            var list = await _repository.GetByDocumentIdAsync(documentId);
            return _mapper.Map<List<DocumentVersionDTO>>(list);
        }
    }
}
