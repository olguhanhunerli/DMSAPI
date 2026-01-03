using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class DocumentVersion
	{
		public int Id { get; set; }

		public int DocumentId { get; set; }
		public Document Document { get; set; }

		public int VersionNumber { get; set; }
		public bool IsLatestVersion { get; set; }

		public string FilePath { get; set; }
		public string? FileName { get; set; }
		public string? OriginalFileName { get; set; }
		public long? FileSize { get; set; }
		public string? FileType { get; set; }

		public string? VersionNote { get; set; }

		public DateTime CreatedAt { get; set; }
		public int CreatedByUserId { get; set; }

		public string FileHash { get; set; }
	}
}
