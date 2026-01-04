using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface IDocumentRevisionRepository: IGenericRepository<DocumentRevision>
    {
        
        Task<DocumentRevision?> GetActiveByDocumentIdAsync(int documentId);
        Task<(List<DocumentRevision>Items, int TotalCount)> GetMyActiveRevisionAsync(int userId, int page, int pageSize);
	}
}
