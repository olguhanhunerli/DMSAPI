using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs
{
    public class RefreshTokenDTO
    {
        public string RawToken { get; set; }    
        public DateTime ExpiresAt { get; set; }
    }
}
