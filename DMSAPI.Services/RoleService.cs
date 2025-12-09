using AutoMapper;
using DMSAPI.Business.Repositories;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Entities.DTOs.RoleDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;

namespace DMSAPI.Services
{
	public class RoleService : IRoleService
	{
		private readonly IRoleRepository _roleRepository;
		private readonly IMapper _mapper;

		public RoleService(IRoleRepository roleRepository, IMapper mapper)
		{
			_roleRepository = roleRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<RoleDTO>> GetAllRoleAsync()
		{
			var roles = await _roleRepository.GetAllRolesAsync();
			return _mapper.Map<IEnumerable<RoleDTO>>(roles);
		}

		public async Task<RoleDTO> GetByIdAsync(int id)
		{
			var role = await _roleRepository.GetByIdAsync(id);
			return _mapper.Map<RoleDTO>(role);
		}

		public async Task AddAsync(AddRoleDTO dto, int userId)
		{
			var role = _mapper.Map<Role>(dto);
			role.CreatedBy = userId;
			await _roleRepository.AddAsync(role);
		}

		public async Task UpdateAsync(int id, UpdateRoleDTO dto, int userId)
		{
			var role = await _roleRepository.GetByIdAsync(id);
			_mapper.Map(dto, role);
			role.UploadedBy = userId;

			await _roleRepository.UpdateAsync(role);
		}

		public async Task DeleteAsync(int id, int userId)
		{
			var role = await _roleRepository.GetByIdAsync(id);
			role.UploadedBy = userId;
			await _roleRepository.DeleteAsync(role);
		}

        public async Task<PagedResultDTO<RoleDTO>> GetPagedAsync(int page, int pageSize)
        {
            var pagedEntities = await _roleRepository.GetPagedAsync(page, pageSize);

            return new PagedResultDTO<RoleDTO>
            {
                TotalCount = pagedEntities.TotalCount,
                Page = pagedEntities.Page,
                PageSize = pagedEntities.PageSize,
                Items = _mapper.Map<List<RoleDTO>>(pagedEntities.Items)
            };
        }
    }
}
