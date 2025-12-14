using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentDTOs
{
    public class DocumentVersionDTO
    {
        public int Id { get; set; }
		public string? FileType { get; set; }
		public string? VersionNote { get; set; }
		public string? CreatedByName { get; set; }
		public string? FileName { get; set; }
		public string? OriginalFileName { get; set; }
	}
}
