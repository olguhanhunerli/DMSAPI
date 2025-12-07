using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;

namespace DMSAPI.Services
{
	public class DepartmentService : IDepartmentService
	{
		private readonly IDepartmentRepository _departmentRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public DepartmentService(
			IDepartmentRepository departmentRepository,
			IMapper mapper,
			IUserRepository userRepository)
		{
			_departmentRepository = departmentRepository;
			_mapper = mapper;
			_userRepository = userRepository;
		}

		public async Task<DepartmentDetailDTO> CreateDepartmentAsync(CreateDepartmentDTO dto, int userId)
		{
			var entity = _mapper.Map<Department>(dto);

			entity.CreatedBy = userId;
			entity.CreatedAt = DateTime.UtcNow;
			entity.IsActive = true;
			entity.IsDeleted = false;

			await _departmentRepository.AddAsync(entity);

			var created = await _departmentRepository.GetDepartmentDetailAsync(entity.Id);
			return _mapper.Map<DepartmentDetailDTO>(created);
		}

		public async Task DeleteDepartmentAsync(int id, int userId)
		{
			var department = await _departmentRepository.GetByIdAsync(id);
			department.UploadedBy = userId;
		    await _departmentRepository.DeleteAsync(department);
		}

		public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync()
		{
			var list = await _departmentRepository.GetAllDepartmentsAsync();
			return _mapper.Map<IEnumerable<DepartmentDTO>>(list);
		}

		public async Task<DepartmentDetailDTO> GetDepartmentByIdAsync(int id)
		{
			var department = await _departmentRepository.GetDepartmentDetailAsync(id)
				?? throw new Exception("Department not found");

			return _mapper.Map<DepartmentDetailDTO>(department);
		}

		public async Task<DepartmentDetailDTO> UpdateDepartmentAsync(UpdateDepartmentDTO dto, int userId)
		{
			var department = await _departmentRepository.GetByIdAsync(dto.Id)
				?? throw new Exception("Department not found");

			_mapper.Map(dto, department);
			department.UploadedBy = userId;
			department.UpdatedAt = DateTime.UtcNow;

			await _departmentRepository.UpdateAsync(department);

			var updated = await _departmentRepository.GetDepartmentDetailAsync(department.Id);
			return _mapper.Map<DepartmentDetailDTO>(updated);
		}
	}
}
