using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface IInstrumentRepository: IGenericRepository<Instrument> 
    {
        Task<PagedResultDTO<Instrument>> GetPagedAsync(int page, int pageSize);
	}
}
