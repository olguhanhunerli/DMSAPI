using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DocumentDTOs;

namespace DMSAPI.Services.IServices
{
	public interface IDocumentService
	{
		Task<DocumentCreateResponseDTO> CreateDocumentAsync(
			DocumentCreateDTO documentCreateDTO,
			int userIdFromToken
		);

		Task<IEnumerable<DocumentDTO>> GetAllDocumentsAsync();
        Task<PagedResultDTO<DocumentDTO>> GetPageAsync(int page, int pageSize);
    }
}
