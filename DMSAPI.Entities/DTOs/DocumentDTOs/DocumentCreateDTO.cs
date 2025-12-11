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
        public string? TitleTr { get; set; }
        public string? TitleEn { get; set; }

        public int CategoryId { get; set; }
        public int? DepartmentId { get; set; }

        public string? DocumentType { get; set; }
        public string? VersionNote { get; set; }

        public int RevisionNumber { get; set; } = 0;

        public bool IsPublic { get; set; }

        public List<int> ApproverUserIds { get; set; } = new();

        public IFormFile? MainFile { get; set; }     // NEW → Ana dosya
        public List<IFormFile>? Attachments { get; set; } = new(); // NEW → Ek dosyalar

    }
}
