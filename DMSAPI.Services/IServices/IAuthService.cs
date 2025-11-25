using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(UserLoginDTO loginDTO);
    }
}
