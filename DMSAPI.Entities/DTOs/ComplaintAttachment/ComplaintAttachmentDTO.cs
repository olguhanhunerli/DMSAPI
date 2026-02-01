using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.ComplaintAttachment
{
    public class ComplaintAttachmentDTO
    {
		public long Id { get; set; }
		public string ComplaintNo { get; set; } = null!;

		public string OriginalFileName { get; set; } = null!;
		public string ContentType { get; set; } = null!;
		public long FileSize { get; set; }
		public DateTime UploadedAt { get; set; }

		public int UploadedBy { get; set; }
		public string? UploadedByName { get; set; }

		public bool IsDeleted { get; set; }
	}
}
