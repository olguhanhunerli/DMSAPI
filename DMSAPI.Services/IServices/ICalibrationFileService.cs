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
    public interface ICalibrationFileService
    {
		Task<PagedResultDTO<InstrumentCalibrationFileDTO>> GetCalibrationFilesByCalibrationIdAsync(int pageNumber, int pageSize);
		Task<InstrumentCalibrationFileDTO?> GetCalibrationFileByIdAsync(ulong fileId);
		Task<InstrumentCalibrationFileDTO> CreateAsync(CreateCalibrationFileDTO createCalibrationFileDTO, int userId);
		Task<InstrumentCalibrationFileDTO> UploadAsync(UploadCalibrationFileDTO uploadCalibrationFileDTO, int userId);

	}
}
