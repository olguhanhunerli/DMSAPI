using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class ComplaintAttachment
    {
		public long Id { get; set; }

		public string ComplaintNo { get; set; } = null!;

		public string OriginalFileName { get; set; } = null!;
		public string StorageKey { get; set; } = null!;
		public string ContentType { get; set; } = null!;
		public long FileSize { get; set; }
		public string? ChecksumSha256 { get; set; }

		public int? UploadedBy { get; set; }
		public User? UploadedByUser { get; set; }
		public DateTime? UploadedAt { get; set; }

		public int? DeletedBy { get; set; }
		public User? DeletedByUser { get; set; }
		public DateTime? DeletedAt { get; set; }
		public bool IsDeleted { get; set; }
		public Complaint Complaint { get; set; } = null!;
	}
}
