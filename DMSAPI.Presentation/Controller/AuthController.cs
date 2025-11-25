using DMSAPI.Entities.DTOs;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDTO)
        {
            try
            {
                var result = await _service.LoginAsync(loginDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest (new { message = ex.Message });
            }
        }
    }
}
