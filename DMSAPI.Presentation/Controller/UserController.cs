using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("GetAllUserInfo")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var users = await _service.GetAllUserAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    stack = ex.ToString()
                });
            }
        }
        [HttpGet("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            try
            {
                var user = await _service.GetUserByEmailAsync(email);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    stack = ex.ToString()
                });
            }
        }
        [HttpGet("GetUserById/{userId}")]
		public async Task<IActionResult> GetUserById(int userId)
		{
			try
			{
				var user = await _service.GetUserByIdAsync(userId);
				return Ok(user);
			}
			catch (Exception ex)
			{
				return BadRequest(new
				{
					message = ex.Message,
					stack = ex.ToString()
				});
			}
		}
		[HttpGet("GetEmployeesByManagerId/{managerId}")]
		public async Task<IActionResult> GetEmployeesByManagerId(int managerId)
		{
			try
			{
				var users = await _service.GetEmployeesByManagerIdAsync(managerId);
				return Ok(users);
			}
			catch (Exception ex)
			{
				return BadRequest(new
				{
					message = ex.Message,
					stack = ex.ToString()
				});
			}
		}
        [HttpPost("SearchUsers")]
		public async Task<IActionResult> SearchUsers([FromBody] UserSearchDTO userSearchDTO)
		{
			try
			{
				var users = await _service.SearchUsersAsync(userSearchDTO);
				return Ok(users);
			}
			catch (Exception ex)
			{
				return BadRequest(new
				{
					message = ex.Message,
					stack = ex.ToString()
				});
			}
		}
		[HttpPost("PasswordResetForAdmin")]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetDTO passwordResetDTO)
        {
            try
            {
                var result = await _service.PasswordResetAsync(passwordResetDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    stack = ex.ToString()
                });
            }
        }
        [HttpPost("PasswordUpdateByUser")]
        public async Task<IActionResult> PasswordUpdate([FromBody] PasswordUpdateDTO passwordUpdateDTO)
        {
            try
            {
                var result = await _service.PasswordUpdateAsync(passwordUpdateDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    stack = ex.ToString()
                });
            }
        }
        [HttpPost("SetActiveStatus")]
        public async Task<IActionResult> SetActiveStatus([FromBody] UserActiveStatusDTO userActiveStatusDTO)
        {
            try
            {
                var result = await _service.SetActiveStatusAsync(userActiveStatusDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    stack = ex.ToString()
                });
            }
        }
		
		[HttpPut("UpdateUser")]
		public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserDTO)
		{
			try
			{
                var userIdFromToken = GetUserId();
                var updatedUser = await _service.UpdateUserAsync(updateUserDTO, userIdFromToken);
				return Ok(updatedUser);
			}
			catch (Exception ex)
			{
				return BadRequest(new
				{
					message = ex.Message,
					stack = ex.ToString()
				});
			}
		}
		[HttpDelete("SoftDeleteUser/{userId}")]
		public async Task<IActionResult> SoftDeleteUser(int userId)
		{
			try
			{
				var deletedUser = await _service.SoftDeleteUser(userId);
				return Ok(deletedUser);
			}
			catch (Exception ex)
			{
				return BadRequest(new
				{
					message = ex.Message,
					stack = ex.ToString()
				});
			}
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