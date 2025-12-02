using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DepartmentDTOs
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DepartmentCode { get; set; }

        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

    }
}
