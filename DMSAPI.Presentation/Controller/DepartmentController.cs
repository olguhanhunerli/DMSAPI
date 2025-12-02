using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController: ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet("get-all-departments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }
        [HttpGet("get-departments-by-company/{companyId}")]
        public async Task<IActionResult> GetDepartmentsByCompanyId(int companyId)
        {
            var departments = await _departmentService.GetDepartmentsByCompanyIdAsync(companyId);
            return Ok(departments);
        }
        [HttpPost("create-department")]
        public async Task<IActionResult> CreateDepartment([FromBody]CreateDepartmentDTO createDepartmentDTO)
        {
            int userIdFromToken = GetUserId();
            var department = await _departmentService.CreateDepartmentAsync(createDepartmentDTO, userIdFromToken);
            return Ok(department);
        }
        [HttpPut("update-department")]
        public async Task<IActionResult> UpdateDepartment([FromBody]UpdateDepartmentDTO updateDepartmentDTO)
        {
            int userIdFromToken = GetUserId();
            var department = await _departmentService.UpdateDepartmentAsync(updateDepartmentDTO, userIdFromToken);
            return Ok(department);
        }
        [HttpGet("get-department-by-id/{departmentId}")]
        public async Task<IActionResult> GetDepartmentById(int departmentId)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(departmentId);
            return Ok(department);
        }
        private int GetUserId()
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userId == null)
                throw new Exception("User ID not found in token");

            return int.Parse(userId);
        }
    }
}
