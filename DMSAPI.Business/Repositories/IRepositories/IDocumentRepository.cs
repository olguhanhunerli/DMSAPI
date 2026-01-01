using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;

namespace DMSAPI.Business.Repositories.IRepositories
{
	public interface IDocumentRepository : IGenericRepository<Document>
	{
		Task<PagedResultDTO<Document>> GetPagedApprovedAsync(int page, int pageSize, int userId, int roleId, int departmentId);
		Task<bool> ValidateDocumentCodeAsync(string documentCode, int companyId, int categoryId);
		Task<PagedResultDTO<Document>> GetPagedAuthorizedAsync(int page,int pageSize,int userId,int roleId,int departmentId);
        Task<List<Document>> GetPendingDocumentIdsForUserAsync(List<int> documentIds);
        Task<PagedResultDTO<Document>> GetPagedPendingByIdsAsync(List<int> documentIds,int page,int pageSize);
        Task AddMainFileAsync(DocumentFile file);
		Task<DocumentFile?> GetMainFileAsync(int documentId);
		Task<Document?> GetDetailByIdAsync(int documentId);
		Task<PagedResultDTO<Document>> GetPagedRejectedAsync(int page, int pageSize);
		Task<Document?> GetDocumentWithFileAsync(int documentId);
		Task<PagedResultDTO<Document>> GetPagedByCategoryAsync(int page, int pageSize, int? categoryId, int roleId, int userId, int departmentId);


	}
}
