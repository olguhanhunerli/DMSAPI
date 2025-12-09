using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentDTOs
{
    public class DocumentCreatePreviewDTO
    {
        public string DocumentCode { get; set; }

        public string CompanyName { get; set; }
        public string CategoryName { get; set; }
        public string CategoryBreadcrumb { get; set; }

        public int VersionNumber { get; set; }
        public string Status { get; set; }

        public string OwnerName { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsCodeValid { get; set; }
    }
}
