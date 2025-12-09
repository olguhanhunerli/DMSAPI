using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IDocumentVersionService
    {
        Task AddAsync(DocumentVersion version);
        Task<List<DocumentVersionDTO>> GetByDocumentIdAsync(int documentId);
    }
}
