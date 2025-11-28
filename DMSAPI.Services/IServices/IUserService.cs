using DMSAPI.Entities.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUserAsync();
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> UpdateUserAsync(UpdateUserDTO updateUserDTO);
        Task<bool> PasswordResetAsync(PasswordResetDTO passwordResetDTO);
        Task<bool> PasswordUpdateAsync(PasswordUpdateDTO passwordUpdateDTO);
        Task<bool> SetActiveStatusAsync(UserActiveStatusDTO userActiveStatusDTO);
    }
}
