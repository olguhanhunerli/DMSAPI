using DMSAPI.Entities.DTOs.CustomerDTO;
using DMSAPI.Presentation.Authorization;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Presentation.Controller
{
	[Authorize(Roles = RoleGroups.InternalRead)]
	[Route("api/[controller]")]
    public class CustomerController : BaseApiController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var customers = await _customerService.GetAllCustomerAsync(page, pageSize);
            return Ok(customers);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }
        [HttpGet("lookup{id}")]
		public async Task<IActionResult> GetCustomerByCompanyId(int id)
		{
			var customer = await _customerService.GetCustomerByCompanyId(id);
			if (customer == null)
				return NotFound();
			return Ok(customer);
		}
		[Authorize(Roles = RoleGroups.MasterDataWrite)]
		[HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDTO customerCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var createdCustomer = await _customerService.CreateCustomerAsync(customerCreateDto);
            return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.Id }, createdCustomer);
        }
		[Authorize(Roles = RoleGroups.MasterDataWrite)]
		[HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDTO customerUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var updatedCustomer = await _customerService.UpdateCustomerAsync(id, customerUpdateDto);
            if (updatedCustomer == null)
                return NotFound();
            return Ok(updatedCustomer);
        }
		[Authorize(Roles = RoleGroups.MasterDataWrite)]
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id, UserId);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}