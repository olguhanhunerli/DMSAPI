using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.InstrumentDTO;
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
        Task<Instrument?> GetByIdAsync(int id);
		Task<PagedResultDTO<Instrument>> GetDeletedByPagedAsync(int page, int pageSize);
		Task<Instrument?> GetDeletedByIdAsync(int id);


	}
}
