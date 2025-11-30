using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface IDocumentRepository: IGenericRepository<Document>
    {
		Task<int> GetNextDocumentNumberAsync(int companyId, int categoryId);
		Task<bool> DocumentCodeExistingAsync(string documentCode);
		Task<bool> ValidateDocumentCodeAsync(string documentCode, int companyId, int categoryId);
	}
}
