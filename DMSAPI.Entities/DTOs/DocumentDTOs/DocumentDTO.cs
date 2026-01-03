using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Entities.DTOs.RoleDTOs;
using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentDTOs
{
    public class DocumentDTO
    {
		public int Id { get; set; }

		public string? Title { get; set; }
		public string DocumentCode { get; set; }

		public int VersionNumber { get; set; }
		public string? VersionNote { get; set; }
		public string? DocumentType { get; set; }
		public bool IsLatestVersion { get; set; }

		public int CategoryId { get; set; }
		public string? CategoryName { get; set; }
		public List<string>? Breadcrumb { get; set; }
		public string? BreadcrumbPath { get; set; }

		public int CompanyId { get; set; }
		public string? CompanyName { get; set; }
		public string? CompanyCode { get; set; }

		public bool IsPublic { get; set; }
		public List<int>? AllowedRoleIds { get; set; }
		public List<int>? AllowedDepartmentIds { get; set; }
		public List<int>? AllowedUserIds { get; set; }
        public List<RoleDTO> AllowedRoles { get; set; } = new();
        public List<DepartmentDTO> AllowedDepartments { get; set; } = new();
        public List<UserMiniDTO> AllowedUsers { get; set; } = new();
        public int StatusId { get; set; }
		public string? Status { get; set; }

		public int? CurrentApproverId { get; set; }
		public string? CurrentApproverName { get; set; }

		public int? ApprovedBy { get; set; }
		public string? ApprovedByName { get; set; }

		public int? RejectedBy { get; set; }
		public string? RejectedByName { get; set; }
		public DateTime? RejectedAt { get; set; }
		public string? RejectReason { get; set; }
		public bool IsLocked { get; set; }
		public int LockedByUserId { get; set; }
		public string LockedByUserName { get; set; }
		public DateTime LockedAt { get; set; }
		public MainDocumentFileDTO? MainFile { get; set; }
		public List<DocumentAttachmentDTO>? Attachments { get; set; }
		public List<DocumentVersionDTO>? Versions { get; set; }

		public List<DocumentApprovalHistoryDTO>? ApprovalHistories { get; set; }
		public List<DocumentAccessLogDTO>? AccessLogs { get; set; }
        

        public DateTime CreatedAt { get; set; }
		public int CreatedByUserId { get; set; }
		public string? CreatedByName { get; set; }

		public DateTime? UpdatedAt { get; set; }
		public int? UpdatedByUserId { get; set; }
		public string? UpdatedByName { get; set; }

		public bool IsDeleted { get; set; }
		public DateTime? DeletedAt { get; set; }
		public int? DeletedByUserId { get; set; }
		public string? DeletedByName { get; set; }

		public bool IsArchived { get; set; }
		public DateTime? ArchivedAt { get; set; }
	}
}
