using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.Models;

namespace DMSAPI.Services.IServices
{
	public interface IDocumentService
	{
		Task<DocumentCreateResponseDTO> CreateDocumentAsync(
			DocumentCreateDTO documentCreateDTO,
			int userIdFromToken
		);
		Task<PagedResultDTO<DocumentDTO>> GetPagedApprovedAsync(int page, int pageSize);
		Task<DocumentCreatePreviewDTO> GetCreatePreviewAsync(int categoryId, int userId);
        Task<IEnumerable<DocumentDTO>> GetAllDocumentsAsync();
        Task<PagedResultDTO<DocumentDTO>> GetPageAsync(int page, int pageSize, int userId, int roleId, int departmentId);
        Task<List<DocumentDTO>> GetMyPendingApprovalsAsync(int userId);
        Task<PagedResultDTO<MyPendingDTO>> GetMyPendingApprovalsAsync(int page,int pageSize,int userId);
		Task<DocumentDTO> GetDetailByIdAsync(int documentId);
		Task<PagedResultDTO<DocumentDTO>> GetRejectedDocumentsAsync(int page, int pageSize);
		Task<DownloadFileResultDTO> DownloadDocumentFileAsync(int documentId);
		Task<DownloadFileResultDTO> DownloadPdfAsync(int documentId);
	}
}
