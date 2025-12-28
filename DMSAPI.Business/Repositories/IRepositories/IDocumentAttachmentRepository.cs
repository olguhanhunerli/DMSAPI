using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface IDocumentAttachmentRepository: IGenericRepository<DocumentAttachment>
    {
        Task<List<DocumentAttachment>> GetByDocumentIdAsync(int documentId);
        Task<DocumentAttachment?> GetMainFileAsync(int documentId);
		Task<DocumentAttachment?> GetByIdAsync(int attachmentId);
	}
}
