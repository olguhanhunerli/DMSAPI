using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.InstrumentCalibrationDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
	public class InstrumentCalibrationService : IInstrumentCalibrationService
	{
		private readonly IInstrumentCalibrationRepository _instrumentCalibrationRepository;
		private readonly IMapper _mapper;

		public InstrumentCalibrationService(IInstrumentCalibrationRepository instrumentCalibrationRepository, IMapper mapper)
		{
			_instrumentCalibrationRepository = instrumentCalibrationRepository;
			_mapper = mapper;
		}

		public async Task<InstrumentCalibrationDTO> CreateCalibrationAsync(CreateCalibrationDTO createCalibrationDTO, int userId)
		{
			var calibrationEntity = _mapper.Map<InstrumentCalibration>(createCalibrationDTO);
			calibrationEntity.CreatedBy = userId;
			calibrationEntity.CreatedAt = DateTime.UtcNow;
			calibrationEntity.UpdatedBy = null;
			calibrationEntity.UpdatedAt = DateTime.UtcNow;
			calibrationEntity.IsActive = true;
			calibrationEntity.IsDeleted = false;
			calibrationEntity.DeletedAt = null;
			calibrationEntity.DeletedBy = null;
			calibrationEntity.DueDate = calibrationEntity.CalibrationDate.AddMonths(createCalibrationDTO.IntervalMonths);
			await _instrumentCalibrationRepository.AddAsync(calibrationEntity);
			return _mapper.Map<InstrumentCalibrationDTO>(calibrationEntity);
		}

		public async Task DeleteCalibrationAsync(ulong id, int userId)
		{
			var calibration = await _instrumentCalibrationRepository.GetByIdAsync(id);
			if (calibration == null)
			{
				throw new Exception("Calibration not found");
			}
			calibration.IsDeleted = true;
			calibration.DeletedAt = DateTime.UtcNow;
			calibration.DeletedBy = userId;
			await _instrumentCalibrationRepository.UpdateAsync(calibration);
		}

		public async Task<InstrumentCalibrationDTO> GetByIdASync(ulong id)
		{
			var result = await _instrumentCalibrationRepository.GetByIdAsync(id);
			if (result == null)
			{
				throw new Exception("Calibration not found");
			}
			return _mapper.Map<InstrumentCalibrationDTO>(result);
		}

		public async Task<PagedResultDTO<InstrumentCalibrationDTO>> GetInstrumentCalibrationsAsync(int pageNumber, int pageSize)
		{
			var result = await _instrumentCalibrationRepository.GetInstrumentCalibrationsAsync(pageNumber, pageSize);

			var dtoItems = _mapper.Map<List<InstrumentCalibrationDTO>>(result.Items);

			return new PagedResultDTO<InstrumentCalibrationDTO>
			{
				Page = result.Page,
				PageSize = result.PageSize,
				TotalCount = result.TotalCount,
				Items = dtoItems
			};

		}

		public async Task<InstrumentCalibrationDTO> UpdateCalibrationAsync(UpdateCalibrationDTO updateCalibrationDTO, int userId)
		{
			var existingCalibration = await _instrumentCalibrationRepository.GetByIdAsync(updateCalibrationDTO.CalibrationId);
			if (existingCalibration == null)
			{
				throw new Exception("Calibration not found");
			}
			_mapper.Map(updateCalibrationDTO, existingCalibration);
			existingCalibration.UpdatedBy = userId;
			existingCalibration.UpdatedAt = DateTime.UtcNow;
			existingCalibration.DueDate = existingCalibration.CalibrationDate.AddMonths(updateCalibrationDTO.IntervalMonths);
			await _instrumentCalibrationRepository.UpdateAsync(existingCalibration);
			return _mapper.Map<InstrumentCalibrationDTO>(existingCalibration);
		}
	}
}
