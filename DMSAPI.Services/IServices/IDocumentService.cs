using DMSAPI.Entities.DTOs.DocumentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IDocumentService
    {
        Task<DocumentCreateResponseDTO> CreateDocumentAsync(DocumentCreateDTO documentCreateDTO, int userIdFromToken);
		Task<IEnumerable<DocumentDTO>> GetAllDocumentsAsync();
	}
}
