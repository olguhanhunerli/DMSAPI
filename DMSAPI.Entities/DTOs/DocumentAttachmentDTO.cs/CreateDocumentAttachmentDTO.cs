using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentAttachmentDTO.cs
{
    public class CreateDocumentAttachmentDTO
    {
        public int DocumentId { get; set; }

        public List<IFormFile> Files { get; set; } = new();

        public bool IsMainFile { get; set; } = false;
    }
}
