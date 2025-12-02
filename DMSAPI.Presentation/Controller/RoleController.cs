using DMSAPI.Entities.DTOs.RoleDTOs;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRoleAsync();
            return Ok(roles);
        }
        [HttpGet("GetRoleById/{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] AddRoleDTO roleDTO)
        {
            var user = GetUserId();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _roleService.AddAsync(roleDTO,user);
            return Ok(roleDTO);
        }
        [HttpPut("UpdateRole/{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleDTO roleDTO)
        {
            var user = GetUserId();
            var existingRole = await _roleService.GetByIdAsync(id);
            if (existingRole == null)
            {
                return NotFound();
            }
            await _roleService.UpdateAsync(id, roleDTO,user);
            return NoContent();
        }
        [HttpDelete("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var user = GetUserId();
            var existingRole = await _roleService.GetByIdAsync(id);
            if (existingRole == null)
            {
                return NotFound();
            }
            await _roleService.DeleteAsync(id, user);
            return Ok("Silme İşlemi Başarılı");
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
