using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.CustomerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface ICustomerService
    {
        Task<PagedResultDTO<CustomerDTO>> GetAllCustomerAsync(int page, int pageSize);
        Task<CustomerDTO?> GetCustomerByIdAsync(int id);
        Task<CustomerDTO> CreateCustomerAsync(CreateCustomerDTO createCustomerDTO);
        Task<CustomerDTO?> UpdateCustomerAsync(int id, UpdateCustomerDTO updateCustomerDTO);
        Task<bool> DeleteCustomerAsync(int id, int userId);
    }
}
