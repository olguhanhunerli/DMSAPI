using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.InstrumentDTO;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

		public async Task BackupDeleteAsync(int id)
		{
			var entity = await _instrumentRepository.GetDeletedByIdAsync(id);
			if (entity == null)
				throw new Exception("Instrument not found");
			entity.IsDeleted = false;
			entity.DeletedAt = null;
			entity.DeletedBy = null;
			entity.Status = "Geri Alındı";
			await _instrumentRepository.UpdateAsync(entity);
		}

		public async Task<InstrumentDTO> CreateAsync(CreateInstrumentDTO createInstrumentDTO, int userId)
		{
			var entity = _mapper.Map<Instrument>(createInstrumentDTO);
			entity.CreatedBy = userId;
			entity.Created_At = DateTime.UtcNow;
			entity.UpdatedBy = userId;
			entity.Updated_At = DateTime.UtcNow;
			entity.IsActive = true;
			entity.IsDeleted = false;
			await _instrumentRepository.AddAsync(entity);
			return _mapper.Map<InstrumentDTO>(entity);
		}

		public async Task DeleteAsync(int id, int userId)
		{
			var entity = await _instrumentRepository.GetByIdAsync(id);
			if(entity == null)
				throw new Exception("Instrument not found");
			entity.IsDeleted = true;
			entity.DeletedAt = DateTime.UtcNow;
			entity.DeletedBy = userId;
			entity.Status = "Silİndi";
			await _instrumentRepository.UpdateAsync(entity);
		}

		public async Task<InstrumentDTO?> GetByIdAsync(int id)
		{
			var entity = await _instrumentRepository.GetByIdAsync(id);
			if (entity == null)
				return null;
			return _mapper.Map<InstrumentDTO>(entity);
		}

		public async Task<InstrumentDTO?> GetDeletedByIdAsync(int id)
		{
			var entity = await _instrumentRepository.GetDeletedByIdAsync(id);
			if (entity == null)
				return null;
			return _mapper.Map<InstrumentDTO>(entity);
		}

		public async Task<PagedResultDTO<InstrumentDTO>> GetDeletedByPagedAsync(int page, int pageSize)
		{
			var result = await _instrumentRepository.GetDeletedByPagedAsync(page, pageSize);
			return new PagedResultDTO<InstrumentDTO>
			{
				TotalCount = result.TotalCount,
				Page = result.Page,
				PageSize = result.PageSize,
				Items = _mapper.Map<List<InstrumentDTO>>(result.Items)
			};
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

		public async Task ToggleIsActiveAsync(int id, bool isActive, int userId)
		{
			var entity = await _instrumentRepository.GetByIdAsync(id);
			if (entity == null)
				throw new Exception("Instrument not found");
			entity.IsActive = isActive;
			entity.Status ="Güncellendi";
			await _instrumentRepository.UpdateAsync(entity);
		}

		public async Task<InstrumentDTO> UpdateAsync(int id, UpdateInstrumentDTO dto, int userId)
		{
			var entity = await _instrumentRepository.GetByIdAsync(id);
			if (entity == null)
				throw new Exception("Instrument not found");
			_mapper.Map(dto, entity);
			entity.UpdatedBy = userId;
			entity.Updated_At = DateTime.UtcNow;
			entity.IsActive = true;
			entity.IsDeleted = false;
			entity.Status = "Güncellendi";
			await _instrumentRepository.UpdateAsync(entity);
			return _mapper.Map<InstrumentDTO>(entity);
		}
	}
}
