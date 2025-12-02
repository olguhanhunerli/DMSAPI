using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.CompanyDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task CreateCompanyAsync(AddCompanyDTO companyCreateDTO, int userIdFromToken)
        {
            var companyEntity = _mapper.Map<Company>(companyCreateDTO);
            companyEntity.CreatedAt = DateTime.UtcNow;
            companyEntity.CreatedBy= userIdFromToken;
            await _companyRepository.AddAsync(companyEntity);
        }

        public Task DeleteCompanyAsync(int companyId, int userIdFromToken)
        {
            var companyEntity = _companyRepository.GetByIdAsync(companyId);
            if (companyEntity == null)
            {
                throw new Exception("Company not found");
            }
            companyEntity.Result.UpdatedAt = DateTime.UtcNow;
            companyEntity.Result.UploadedBy = userIdFromToken;
            return _companyRepository.DeleteAsync(companyEntity.Result);
        }

        public async Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            var companyDtos = _mapper.Map<IEnumerable<CompanyDTO>>(companies);
            return companyDtos;
        }

        public async Task<CompanyDTO> GetCompanyByIdAsync(int companyId)
        {
            var companyEntity = await _companyRepository.GetByIdAsync(companyId);
            if (companyEntity == null)
            {
                throw new Exception("Company not found");
            }
            return _mapper.Map<CompanyDTO>(companyEntity);

        }

        public async Task UpdateCompanyAsync(int companyId, UpdateCompanyDTO companyUpdateDTO, int userIdFromToken)
        {
            var existingCompany =await _companyRepository.GetByIdAsync(companyId);
            if (existingCompany == null)
            {
                throw new Exception("Company not found");
            }
            
            _mapper.Map(companyUpdateDTO, existingCompany);
            existingCompany.UploadedBy = userIdFromToken;
            existingCompany.UpdatedAt = DateTime.UtcNow;
            _companyRepository.UpdateAsync(existingCompany);
        }
    }
}
