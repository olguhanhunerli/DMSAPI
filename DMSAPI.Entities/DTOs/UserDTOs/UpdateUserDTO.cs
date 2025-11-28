using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.UserDTOs
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
        [DefaultValue("Ahmet")]
        public string? FirstName { get; set; }

        [DefaultValue("Yılmaz")]
        public string? LastName { get; set; }

        [DefaultValue("ahmet@example.com")]
        public string? Email { get; set; }

        [DefaultValue("+90555555555")]
        public string? PhoneNumber { get; set; }

        [DefaultValue("ahmety")]
        public string? UserName { get; set; }


        [DefaultValue(1)]
        public int? RoleId { get; set; }

        [DefaultValue(101)]
        public int? DepartmentId { get; set; }

        [DefaultValue(1)]
        public int? CompanyId { get; set; }

        [DefaultValue(1007)]
        public int? ManagerId { get; set; }

        [DefaultValue("Senior Developer")]
        public string? Position { get; set; }

        [DefaultValue(true)]
        public bool? CanApprove { get; set; }

        [DefaultValue(3)]
        public int? ApprovalLevel { get; set; }

        [DefaultValue(true)]
        public bool? IsActive { get; set; }

        [DefaultValue(false)]
        public bool? IsLocked { get; set; }

        [DefaultValue("tr")]
        public string? Language { get; set; }

        [DefaultValue("Europe/Istanbul")]
        public string? TimeZone { get; set; }

        [DefaultValue("light")]
        public string? Theme { get; set; }

        [DefaultValue("email,push")]
        public string? NotificationPreferences { get; set; }
    }
}
