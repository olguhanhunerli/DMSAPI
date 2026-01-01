using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface IDocumentCodeReservationRepository: IGenericRepository<DocumentCodeReservation>
    {
        Task<DocumentCodeReservation?> ReserveNextCodeAsync(int companyId, int categoryId, string companyCode, string categoryCode, int userId);
        Task MarkAsUsedAsync(string documentCode);
		Task<DocumentCodeReservation?> GetByCodeAsync(string documentCode);
	}
}
