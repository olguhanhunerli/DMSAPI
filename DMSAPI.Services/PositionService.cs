using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.PositionDTOs;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<PositionDTO>> GetAllPositionsAsync()
        {
            var positions = await _positionRepository.GetAllAsync();
            var positionDTOs = _mapper.Map<IEnumerable<PositionDTO>>(positions);
            return positionDTOs;
        }
    }
}
