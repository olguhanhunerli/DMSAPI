using AutoMapper;
using DMSAPI.Business.Repositories;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Entities.DTOs.PositionDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;

namespace DMSAPI.Services
{
	public class PositionService : IPositionService
	{
		private readonly IPositionRepository _positionRepository;
		private readonly IMapper _mapper;

		public PositionService(IPositionRepository positionRepository, IMapper mapper)
		{
			_positionRepository = positionRepository;
			_mapper = mapper;
		}

		public async Task AddAsync(CreatePositionDTO positionDTO, int userId)
		{
			var position = _mapper.Map<Position>(positionDTO);
			position.CreatedBy = userId;
			await _positionRepository.AddAsync(position);
		}

		public Task AddPositionAsync(CreatePositionDTO positionDTO, int userIdFromToken)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<PositionDTO>> GetAllPositionsAsync()
		{
			var list = await _positionRepository.GetAllPositionsAsync();
			return _mapper.Map<IEnumerable<PositionDTO>>(list);
		}

		public async Task<PositionDTO> GetPositionByIdAsync(int id)
		{
			var position = await _positionRepository.GetPositionByIdAsync(id)
				?? throw new Exception("Position not found");

			return _mapper.Map<PositionDTO>(position);
		}

		public async Task<PositionDTO> UpdatePositionAsync(UpdatePositionDTO positionDTO, int userIdFromToken)
		{
			var position = await _positionRepository.GetByIdAsync(positionDTO.Id)
				?? throw new Exception("Position not found");
			_mapper.Map(positionDTO, position);
			position.UploadedBy = userIdFromToken;
			position.UploadedAt = DateTime.UtcNow;

			await _positionRepository.UpdateAsync(position);
			var updated = await _positionRepository.GetByIdAsync(positionDTO.Id);
			return _mapper.Map<PositionDTO>(updated);
		}
	}
}
