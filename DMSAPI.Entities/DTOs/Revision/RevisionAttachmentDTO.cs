using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.Revision
{
    public class RevisionAttachmentDTO
    {
		public int Id { get; set; }
		public string FileName { get; set; } = null!;
		public string ContentType { get; set; } = null!;
		public long FileSize { get; set; }
		public DateTime UploadedAt { get; set; }
		public string UploadedByName { get; set; } = null!;
	}
}
