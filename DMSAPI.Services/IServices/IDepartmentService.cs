using DMSAPI.Entities.DTOs.DepartmentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync();
        Task<IEnumerable<DepartmentDTO>> GetDepartmentsByCompanyIdAsync(int companyId);
        Task<DepartmentDetailDTO> GetDepartmentByIdAsync(int id);

        Task<DepartmentDetailDTO> CreateDepartmentAsync(CreateDepartmentDTO createDepartmentDTO, int userIdFromToken);
        Task<DepartmentDetailDTO> UpdateDepartmentAsync(UpdateDepartmentDTO updateDepartmentDTO, int userIdFromToken);
    }
}
