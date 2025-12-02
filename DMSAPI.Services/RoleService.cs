using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.RoleDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var roleDtos = _mapper.Map<IEnumerable<RoleDTO>>(roles);
            return roleDtos;
        }
       
        public async Task<RoleDTO>? GetByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            return _mapper.Map<RoleDTO>(role);
        }
        public async Task AddAsync(AddRoleDTO roleDTO, int userIdFromToken)
        {
            var role = _mapper.Map<Role>(roleDTO);
            role.CreatedBy = userIdFromToken;
            await _roleRepository.AddAsync(role);
        }
        public async Task UpdateAsync(int id, UpdateRoleDTO roleDTO, int userIdFromToken)
        {
            var existingRole = await _roleRepository.GetByIdAsync(id);
                if (existingRole != null)
            existingRole.UploadedBy = userIdFromToken;
            _mapper.Map(roleDTO, existingRole);
            await _roleRepository.UpdateAsync(existingRole);
            
        }
        public async Task DeleteAsync(int id, int userIdFromToken)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role != null)
            {
                role.UploadedBy = userIdFromToken;
                await _roleRepository.DeleteAsync(role);
            }
        }
    }
}
