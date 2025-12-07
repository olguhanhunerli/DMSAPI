using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.CompanyDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;

namespace DMSAPI.Services
{
	public class CompanyService : ICompanyService
	{
		private readonly ICompanyRepository _companyRepository;
		private readonly IMapper _mapper;

		public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
		{
			_companyRepository = companyRepository;
			_mapper = mapper;
		}

		public async Task CreateCompanyAsync(AddCompanyDTO dto, int userId)
		{
			var company = _mapper.Map<Company>(dto);
			company.CreatedBy = userId;
			company.CreatedAt = DateTime.UtcNow;

			await _companyRepository.AddAsync(company);
		}

		public async Task DeleteCompanyAsync(int companyId, int userId)
		{
			var company = await _companyRepository.GetByIdAsync(companyId)
				?? throw new Exception("Company not found");

			company.UpdatedAt = DateTime.UtcNow;
			company.UploadedBy = userId;

			await _companyRepository.DeleteAsync(company);
		}

		public async Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync()
		{
			var companies = await _companyRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<CompanyDTO>>(companies);
		}

		public async Task<CompanyDTO> GetCompanyByIdAsync(int companyId)
		{
			var company = await _companyRepository.GetByIdAsync(companyId)
				?? throw new Exception("Company not found");

			return _mapper.Map<CompanyDTO>(company);
		}

		public async Task UpdateCompanyAsync(int companyId, UpdateCompanyDTO dto, int userId)
		{
			var company = await _companyRepository.GetByIdAsync(companyId)
				?? throw new Exception("Company not found");

			_mapper.Map(dto, company);
			company.UpdatedAt = DateTime.UtcNow;
			company.UploadedBy = userId;

			await _companyRepository.UpdateAsync(company);
		}
	}
}
