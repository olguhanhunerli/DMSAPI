using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface IDocumentApprovalRepository: IGenericRepository<DocumentApproval>
    {
        Task<List<DocumentApproval>> GetByDocumentIdAsync(int documentId);
        Task<List<int>> GetPendingDocumentIdsAsync(int userId);
        Task<DocumentApproval> GetNextPendingApprovalAsync(int documentId);
    }
}
