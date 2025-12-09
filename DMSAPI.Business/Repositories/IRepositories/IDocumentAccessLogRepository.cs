using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface IDocumentAccessLogRepository: IGenericRepository<DocumentAccessLog>
    {
        Task<List<DocumentAccessLog>> GetByDocumentIdAsync(int documentId);
    }
}
