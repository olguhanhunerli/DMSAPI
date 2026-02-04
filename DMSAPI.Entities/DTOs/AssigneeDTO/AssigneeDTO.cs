using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.AssigneeDTO
{
    public class AssigneeDTO
    {
        public int UserId { get; set; }
        public bool IsPrimary { get; set; }
        public string? UserName { get; set; }
    }
}
