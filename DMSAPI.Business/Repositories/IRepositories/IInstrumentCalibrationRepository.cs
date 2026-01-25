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
    public interface IInstrumentCalibrationRepository: IGenericRepository<InstrumentCalibration>
	{
        Task<PagedResultDTO<InstrumentCalibration>> GetInstrumentCalibrationsAsync(int pageNumber, int pageSize);
		Task <InstrumentCalibration> GetByIdAsync(ulong id);
		Task<InstrumentCalibration?> GetForUpdateAsync(ulong id);
	}
}
