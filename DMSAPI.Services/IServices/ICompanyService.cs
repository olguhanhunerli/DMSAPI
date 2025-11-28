using DMSAPI.Entities.DTOs.CompanyDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDTO>> GetAllCompaniesAsync();
        Task<CompanyDTO> GetCompanyByIdAsync(int companyId);
        Task CreateCompanyAsync(AddCompanyDTO companyCreateDTO);
        Task UpdateCompanyAsync(int companyId, UpdateCompanyDTO companyUpdateDTO);
        Task DeleteCompanyAsync(int companyId);
    }
}
