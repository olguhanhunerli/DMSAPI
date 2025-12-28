using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentDTOs
{
    public class MainDocumentFileDTO
    {
		public int Id { get; set; }
		public string FileName { get; set; }
		public string OriginalFileName { get; set; }
		public string FileExtension { get; set; }
		public long FileSize { get; set; }
		public string FilePath { get; set; }
		public string PdfFilePath { get; set; }
		public string AttachmentFilePath { get; set; }
	}
}
