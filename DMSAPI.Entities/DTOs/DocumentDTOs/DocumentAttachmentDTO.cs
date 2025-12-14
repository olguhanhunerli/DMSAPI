using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentDTOs
{
    public class DocumentAttachmentDTO
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }

        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public long FileSize { get; set; }
        public string? FileType { get; set; }
        public string? FilePath { get; set; }

        public DateTime UploadedAt { get; set; }
        public int? UploadedByUserId { get; set; }
        public string? UploadedByName { get; set; }

        public string FileSizeText { get; set; }
    }
}
