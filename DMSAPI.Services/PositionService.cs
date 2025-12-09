using AutoMapper;
using DMSAPI.Business.Repositories;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
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

		

		public async Task AddPositionAsync(CreatePositionDTO positionDTO, int userIdFromToken)
		{
            var position = _mapper.Map<Position>(positionDTO);

            position.CreatedBy = userIdFromToken;
            position.CreatedAt = DateTime.UtcNow;

            await _positionRepository.AddAsync(position);
        }

        public async Task DeletePositionAsync(int id, int userId)
        {
			var position = await _positionRepository.GetByIdAsync(id);
			position.UploadedBy = userId;
			await _positionRepository.DeleteAsync(position);
        }

        public async Task<IEnumerable<PositionDTO>> GetAllPositionsAsync()
		{
			var list = await _positionRepository.GetAllPositionsAsync();
			return _mapper.Map<IEnumerable<PositionDTO>>(list);
		}

        public async Task<PagedResultDTO<PositionDTO>> GetPagedAsync(int page, int pageSize)
        {
            var pagedEntities = await _positionRepository.GetPagedAsync(page, pageSize);

            return new PagedResultDTO<PositionDTO>
            {
                TotalCount = pagedEntities.TotalCount,
                Page = pagedEntities.Page,
                PageSize = pagedEntities.PageSize,
                Items = _mapper.Map<List<PositionDTO>>(pagedEntities.Items)
            };
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
