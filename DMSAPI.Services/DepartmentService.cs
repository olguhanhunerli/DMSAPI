using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper, IUserRepository userRepository)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<DepartmentDetailDTO> CreateDepartmentAsync(CreateDepartmentDTO dto, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");
            bool exists = await _departmentRepository.IsNameExistAsync(dto.Name, dto.CompanyId);
            if (exists)
                throw new Exception("Department name already exists in this company.");

            bool codeExists = await _departmentRepository.IsCodeExistAsync(dto.DepartmentCode, dto.CompanyId);
            if (codeExists)
                throw new Exception("Department code already exists in this company.");

            var entity = _mapper.Map<Department>(dto);

            entity.DepartmentCode = dto.DepartmentCode;

            entity.CreatedBy = userId;
            entity.UploadedBy = userId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = null;

            entity.IsActive = true;
            entity.IsDeleted = false;

            await _departmentRepository.AddAsync(entity);

            var createdDept = await _departmentRepository.GetDepartmentDetailAsync(entity.Id);

            return _mapper.Map<DepartmentDetailDTO>(createdDept);
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllDepartmentsAsync();
            return _mapper.Map<IEnumerable<DepartmentDTO>>(departments);
        }

        public async Task<IEnumerable<DepartmentDTO>> GetDepartmentsByCompanyIdAsync(int companyId)
        {
            var departments = await _departmentRepository.GetDepartmentsByCompanyAsync(companyId);
            return _mapper.Map<IEnumerable<DepartmentDTO>>(departments);
        }

        public async Task<DepartmentDetailDTO> GetDepartmentByIdAsync(int id)
        {
            var department = await _departmentRepository.GetDepartmentDetailAsync(id);

            if (department == null)
                throw new Exception("Department not found.");

            return _mapper.Map<DepartmentDetailDTO>(department);
        }

        public async Task<DepartmentDetailDTO> UpdateDepartmentAsync(UpdateDepartmentDTO dto, int userId)
        {
            var existing = await _departmentRepository.GetByIdAsync(dto.Id);

            if (existing == null)
                throw new Exception("Department not found.");

            if (existing.Name != dto.Name)
            {
                bool exists = await _departmentRepository.IsNameExistAsync(dto.Name, dto.CompanyId);
                if (exists)
                    throw new Exception("Department name already exists in this company.");
            }

            if (existing.DepartmentCode != dto.DepartmentCode)
            {
                bool codeExists = await _departmentRepository.IsCodeExistAsync(dto.DepartmentCode, dto.CompanyId);
                if (codeExists)
                    throw new Exception("Department code already exists in this company.");
            }

            _mapper.Map(dto, existing);

            existing.UploadedBy = userId;
            existing.UpdatedAt = DateTime.UtcNow;

            await _departmentRepository.UpdateAsync(existing);
            var updated = await _departmentRepository.GetDepartmentDetailAsync(existing.Id);

            return _mapper.Map<DepartmentDetailDTO>(updated);
        }
    }
}
