using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.RoleDTOs
{
    public class AddRoleDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
