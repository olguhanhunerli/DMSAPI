using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentDTOs
{
    public class DocumentCreateResponseDTO
    {
		public int Id { get; set; }

		public string Title { get; set; }
		public string DocumentCode { get; set; }
		public string FileName { get; set; }
		public string OriginalFileName { get; set; }

		public long FileSize { get; set; }
		public string FileType { get; set; }

		public int VersionNumber { get; set; }
		public string VersionNote { get; set; }

		public int CompanyId { get; set; }
		public string CompanyName { get; set; }

		public int CategoryId { get; set; }
		public string CategoryName { get; set; }

		public List<string> Breadcrumb { get; set; }
		public string BreadcrumbPath { get; set; }

		public DateTime CreatedAt { get; set; }
		public int CreatedBy { get; set; }
		public string CreatedByName { get; set; }
	}
}
