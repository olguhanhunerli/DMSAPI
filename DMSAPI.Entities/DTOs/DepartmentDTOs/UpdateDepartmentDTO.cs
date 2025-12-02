using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DepartmentDTOs
{
    public class UpdateDepartmentDTO
    {
        public int Id { get; set; }                   

        public string Name { get; set; }
        public string DepartmentCode { get; set; }

        public int CompanyId { get; set; }
        public int? ManagerId { get; set; }

        public string? Description { get; set; }
        public string? Notes { get; set; }

        public int? SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
