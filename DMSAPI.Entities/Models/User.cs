using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string UserName { get; set; }
        [Required] public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsLocked { get; set; }
        public int FailedLoginCount { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        public string Position { get; set; }
        public string Permissions { get; set; } 

        public bool CanApprove { get; set; }
        public int? ApprovalLevel { get; set; }
        public string SignatureImageUrl { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int? ManagerId { get; set; }
        public User Manager { get; set; }

        public string Language { get; set; }
        public string TimeZone { get; set; }
        public string Theme { get; set; }
        public string NotificationPreferences { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        public ICollection<Permission> PermissionsList { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
