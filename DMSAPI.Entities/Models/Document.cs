using DMSAPI.Entities.Owned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class Document : ICompanyOwned
	{
		public int Id { get; set; }

		public string Title { get; set; }
		public string DocumentCode { get; set; }
		public string FileName { get; set; }
		public string OriginalFileName { get; set; }

		public long FileSize { get; set; }
		public string FileType { get; set; }

		public int CompanyId { get; set; }
		public Company Company { get; set; }

		public int CategoryId { get; set; }
		public Category Category { get; set; }

		public int VersionNumber { get; set; } = 1;
		public string VersionNote { get; set; }

		public bool IsLatestVersion { get; set; } = true;

		public string DocumentType { get; set; }

		public bool IsPublic { get; set; } = false;
		public string AllowedRoles { get; set; }
		public string AllowedDepartments { get; set; }
		public string AllowedUsers { get; set; }

		public string Status { get; set; }

		public int? ApproverId { get; set; }
		public User ApproverUser { get; set; }

		public int? ApprovedBy { get; set; }
		public User ApprovedByUser { get; set; }
		public DateTime? ApprovedAt { get; set; }

		public int? RejectedBy { get; set; }
		public User RejectedByUser { get; set; }
		public DateTime? RejectedAt { get; set; }
		public string RejectReason { get; set; }

		public bool IsDeleted { get; set; } = false;
		public DateTime? DeletedAt { get; set; }
		public int? DeletedBy { get; set; }
		public User DeletedByUser { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public int CreatedBy { get; set; }
		public User CreatedByUser { get; set; }

		public DateTime? UpdatedAt { get; set; }
		public int? UpdatedBy { get; set; }
		public User UpdatedByUser { get; set; }
	}
}
