using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface ICustomerRepository: IGenericRepository<Customer>
    {
        Task<PagedResultDTO<Customer>> GetAllCustomerAsync(int page, int pageSize);
        Task <Customer?> GetCustomerByIdAsync(int id);
    }
}
