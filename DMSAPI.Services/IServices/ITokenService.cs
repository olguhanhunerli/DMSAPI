using DMSAPI.Entities.DTOs;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        RefreshTokenDTO GenerateRefreshToken();
        string HashToken(string token); 
    }
}
