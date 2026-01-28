using AutoMapper;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.CustomerDTO;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDTO> CreateCustomerAsync(CreateCustomerDTO createCustomerDTO)
        {
            var entity = _mapper.Map<Entities.Models.Customer>(createCustomerDTO);
            await _customerRepository.AddAsync(entity);
            return _mapper.Map<CustomerDTO>(entity);
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var entity = await _customerRepository.GetCustomerByIdAsync(id);
            if (entity == null)
                return false;
            await _customerRepository.DeleteAsync(entity);
            return true;
        }

        public async Task<PagedResultDTO<CustomerDTO>> GetAllCustomerAsync(int page, int pageSize)
        {
           var pagedCustomers = await _customerRepository.GetAllCustomerAsync(page, pageSize);
           var customerDTOs = _mapper.Map<List<CustomerDTO>>(pagedCustomers.Items);
                return new PagedResultDTO<CustomerDTO>
                {
                    Items = customerDTOs,
                    TotalCount = pagedCustomers.TotalCount,
                    Page = pagedCustomers.Page,
                    PageSize = pagedCustomers.PageSize
                };
        }

        public async Task<CustomerDTO?> GetCustomerByIdAsync(int id)
        {
            var entity = await _customerRepository.GetCustomerByIdAsync(id);
            if (entity == null)
                return null;
            return _mapper.Map<CustomerDTO>(entity);
        }

        public async Task<CustomerDTO?> UpdateCustomerAsync(int id, UpdateCustomerDTO updateCustomerDTO)
        {
            var entity = await _customerRepository.GetCustomerByIdAsync(id);
            if (entity == null)
                return null;
            _mapper.Map(updateCustomerDTO, entity);
            await _customerRepository.UpdateAsync(entity);
            return _mapper.Map<CustomerDTO>(entity);
        }
    }
}
