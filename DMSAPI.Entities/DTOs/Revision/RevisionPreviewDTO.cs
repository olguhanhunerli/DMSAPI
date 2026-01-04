using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.Revision
{
    public class RevisionPreviewDTO
	{
		public int? DocumentId { get; set; }
		public string DocumentCode { get; set; } = null!;

		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public string CategoryBreadcrumb { get; set; }
		
		public int CompanyId { get; set; }
		public string CompanyName { get; set; }

		public int VersionNumber { get; set; }
		public string? VersionNote { get; set; }

		public int StatusId { get; set; }
		public string Status { get; set; }

		public int OwnerUserId { get; set; }
		public string OwnerName { get; set; }
		public DateTime CreatedAt { get; set; }

		public bool IsCodeValid { get; set; } = true;
		public bool IsRevision { get; set; } = false;
		public List<RevisionAttachmentDTO> Attachments { get; set; }
	}
}
