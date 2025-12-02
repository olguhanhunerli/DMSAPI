using DMSAPI.Entities.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DepartmentDTOs
{
    public class DepartmentDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DepartmentCode { get; set; }

        public int? ManagerId { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public string ManagerEmail { get; set; }

        public string? Description { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
        public int? SortOrder { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int? UploadedBy { get; set; }

        public List<UserMiniDTO> Users { get; set; }
    }
}
