using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CapaEvidenceFiles
{
    public class CapaEvidenceFilesDTO
    {
		public int Id { get; set; }

		public string CapaNo { get; set; }

		public string FileName { get; set; } = string.Empty;

		public string FilePath { get; set; } = string.Empty;

		public string FileType { get; set; } = string.Empty;

		public DateTime UploadedAt { get; set; }

		public string UploadedBy { get; set; } = string.Empty;
	}
}
