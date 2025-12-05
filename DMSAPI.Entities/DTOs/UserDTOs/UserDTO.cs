using DMSAPI.Entities.DTOs.PermissionDTOs;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.UserDTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }

        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public string? ManagerName { get; set; }

        public int? PositionId { get; set; }
        public string? PositionName { get; set; }
        public bool? CanApprove { get; set; }
        public int? ApprovalLevel { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public string? Language { get; set; }
        public string? TimeZone { get; set; }
        public string? Theme { get; set; }
        public string? NotificationPreferences { get; set; }
    }
}
