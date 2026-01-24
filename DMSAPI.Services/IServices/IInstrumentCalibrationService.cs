using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.InstrumentCalibrationDTOs;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IInstrumentCalibrationService
    {
		Task<PagedResultDTO<InstrumentCalibrationDTO>> GetInstrumentCalibrationsAsync(int pageNumber, int pageSize);
		Task <InstrumentCalibrationDTO>CreateCalibrationAsync(CreateCalibrationDTO createCalibrationDTO, int userId);
		Task<InstrumentCalibrationDTO> UpdateCalibrationAsync(UpdateCalibrationDTO updateCalibrationDTO, int userId);
		Task <InstrumentCalibrationDTO> GetByIdASync(ulong id);
		Task DeleteCalibrationAsync(ulong id, int userId);

	}
}
