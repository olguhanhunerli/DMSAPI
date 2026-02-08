using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CapaActionFileDTO
{
    public class CapaActionFilesDTO
    {
		public long Id { get; set; }

		public long ActionId { get; set; }

		public string FileName { get; set; } = default!;

		public string FilePath { get; set; } = default!;

		public string? FileType { get; set; }

		public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

		public long UploadedBy { get; set; }

	}
}
