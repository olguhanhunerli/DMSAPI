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
	}
}
