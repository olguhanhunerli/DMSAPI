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
	}
}
