using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;

namespace DMSAPI.Business.Repositories.IRepositories
{
	public interface IDocumentRepository : IGenericRepository<Document>
	{
		Task<int> GetNextDocumentNumberAsync(int companyId, int categoryId);
		Task<bool> DocumentCodeExistingAsync(string documentCode);
		Task<bool> ValidateDocumentCodeAsync(string documentCode, int companyId, int categoryId);
		Task<PagedResultDTO<Document>> GetPageAsync(int page, int pageSize);
	}
}
