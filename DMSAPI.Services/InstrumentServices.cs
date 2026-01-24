using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.InstrumentDTO;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
	public class InstrumentServices : IInstrumentServices
	{
		private readonly IInstrumentRepository _instrumentRepository;
		private readonly IMapper _mapper;

		public InstrumentServices(IInstrumentRepository instrumentRepository, IMapper mapper)
		{
			_instrumentRepository = instrumentRepository;
			_mapper = mapper;
		}

		public async Task<PagedResultDTO<InstrumentDTO>> GetPagedAsync(int page, int pageSize)
		{
			var result = await _instrumentRepository.GetPagedAsync(page, pageSize);
			return new PagedResultDTO<InstrumentDTO>
			{
				TotalCount = result.TotalCount,
				Page = result.Page,
				PageSize = result.PageSize,
				Items = _mapper.Map<List<InstrumentDTO>>(result.Items)
			};
		}
	}
}
