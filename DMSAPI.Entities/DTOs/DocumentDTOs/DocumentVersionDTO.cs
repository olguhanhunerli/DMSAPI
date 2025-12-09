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
        public int DocumentId { get; set; }

        public int VersionNumber { get; set; }

        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }

        public string VersionNote { get; set; }

        public DateTime CreatedAt { get; set; }
        public int? CreatedByUserId { get; set; }
        public string CreatedByName { get; set; }

        public string FileSizeText { get; set; }
    }
}
