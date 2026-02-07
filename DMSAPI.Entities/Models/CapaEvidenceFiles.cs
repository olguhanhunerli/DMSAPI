using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class CapaEvidenceFiles
    {
		public int Id { get; set; }

		public string CapaNo { get; set; }
		public string FileName { get; set; } = string.Empty;

		public string FilePath { get; set; } = string.Empty;

		public string FileType { get; set; } = string.Empty;

		public DateTime UploadedAt { get; set; }

		public int UploadedBy { get; set; } 
	}
}
