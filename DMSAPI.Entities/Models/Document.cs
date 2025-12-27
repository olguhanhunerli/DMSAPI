using DMSAPI.Entities.Owned;
using System;
using System.Collections.Generic;

namespace DMSAPI.Entities.Models
{
    public class Document : ICompanyOwned
    {
		public int Id { get; set; }

		public string Title { get; set; }
		public string DocumentCode { get; set; }

		public int CompanyId { get; set; }
		public Company Company { get; set; }

		public int CategoryId { get; set; }
		public Category Category { get; set; }

		public int CreatedByUserId { get; set; }
		public User CreatedByUser { get; set; }

		public int? UpdatedByUserId { get; set; }
		public User? UpdatedByUser { get; set; }

		public int? DeletedByUserId { get; set; }
		public User? DeletedByUser { get; set; }

		public int VersionNumber { get; set; }
		public string? VersionNote { get; set; }

		public string? DocumentType { get; set; }

		public bool IsPublic { get; set; }

		public string? AllowedRoles { get; set; }
		public string? AllowedDepartments { get; set; }
		public string? AllowedUsers { get; set; }

		public int StatusId { get; set; }

		public bool IsDeleted { get; set; }
		public bool IsArchived { get; set; }
		public bool IsLatestVersion { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public DateTime? ArchivedAt { get; set; }

		public ICollection<DocumentFile> Files { get; set; } 
		public ICollection<DocumentAttachment> Attachments { get; set; } 
		public ICollection<DocumentVersion> Versions { get; set; } 
		public ICollection<DocumentApproval> Approvals { get; set; }
		public ICollection<DocumentApprovalHistory> ApprovalHistories { get; set; }
		public ICollection<DocumentAccessLog> AccessLogs { get; set; } 
	}
}
