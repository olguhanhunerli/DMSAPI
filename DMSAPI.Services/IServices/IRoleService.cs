using DMSAPI.Entities.DTOs.RoleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetAllRoleAsync();
        Task<RoleDTO>? GetByIdAsync(int id);
        Task AddAsync(AddRoleDTO roleDTO);
        Task UpdateAsync(int id, UpdateRoleDTO roleDTO);
        Task DeleteAsync(int id);
    }
}
