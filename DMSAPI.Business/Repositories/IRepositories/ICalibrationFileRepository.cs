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
    public interface ICalibrationFileRepository: IGenericRepository<DMSAPI.Entities.Models.InstrumentCalibrationFile>
	{
        Task <PagedResultDTO<DMSAPI.Entities.Models.InstrumentCalibrationFile>> GetCalibrationFilesByCalibrationIdAsync(int pageNumber, int pageSize);
		Task<InstrumentCalibrationFile?> GetCalibrationFileByIdAsync(ulong fileId);
	}
}
