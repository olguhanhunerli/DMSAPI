using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.ComplaintDTO;
using DMSAPI.Entities.DTOs.CustomerDTO;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface ICAPARepository : IGenericRepository<CAPA>
    {
        Task<PagedResultDTO<CAPA>> GetAllCAPAAsync(int page, int pageSize);
        Task<CAPA> GetCAPAByCapaNoAsync(string capaNo);
        Task<string?> GetCompanyCodeAsync(int companyId);
        Task<ComplaintDTO?> GetComplaintDtoByNoAsync(string complaintNo);
        Task<CustomerMiniDTO?> GetCustomerMiniByIdAsync(int customerId);

        Task<List<LookupItemDTO>> GetRootCauseMethodLookupsAsync();
        Task<List<RootCauseMethod>> GetRootCouseMethodAsync();
        Task<bool> RootCauseMethodExistsAsync(int id);
        Task<bool> ComplaintExistsAsync(string complaintNo);
    }
}
