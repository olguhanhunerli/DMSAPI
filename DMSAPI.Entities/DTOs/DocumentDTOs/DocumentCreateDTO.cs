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
		public string DocumentCode { get; set; }
		public string? TitleTr { get; set; }
        public string? TitleEn { get; set; }

        public int CategoryId { get; set; }
        public int? DepartmentId { get; set; }

        public string? DocumentType { get; set; }
        public string? VersionNote { get; set; }

        public int RevisionNumber { get; set; } = 0;

        public bool IsPublic { get; set; }

        public List<int> ApproverUserIds { get; set; } = new();
        public List<int>? AllowedDepartmentIds { get; set; }
        public List<int>? AllowedRoleIds { get; set; }
        public List<int>? AllowedUserIds { get; set; }

        public IFormFile? MainFile { get; set; }    
        public List<IFormFile>? Attachments { get; set; } = new(); 

    }
}
