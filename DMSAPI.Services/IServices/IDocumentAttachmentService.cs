using DMSAPI.Entities.DTOs.DocumentAttachmentDTO.cs;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IDocumentAttachmentService
    {
        Task AddAsync(DocumentAttachment attachment);
        Task<List<DocumentAttachmentDTO>> GetByDocumentIdAsync(int documentId);
        Task UploadMultipleAsync(CreateDocumentAttachmentDTO dto, int userId);
    }
}
