using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface ICAPAActionRepository: IGenericRepository<CAPAACTION>
    {
        Task<List<CAPAACTION>> GetByCapaNoAsync(string capaNo);
        Task<CAPAACTION?> GetByIdLongAsync(long id);
        Task<int> CountNotDoneAsync(string capaNo);
    }
}
