using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentDTOs
{
    public class DocumentCreateDTO
    {
		public string Title { get; set; }

		public int CompanyId { get; set; }
		public int CategoryId { get; set; }

		public string DocumentType { get; set; }
		public bool IsPublic { get; set; } = false;

		public List<int> AllowedRoleIds { get; set; }
		public List<int> AllowedDepartmentIds { get; set; }
		public List<int> AllowedUserIds { get; set; }

		public string VersionNote { get; set; }

		public IFormFile File { get; set; }

	}
}
