using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class DocumentAttachment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DocumentId { get; set; }

        [StringLength(255)]
        public string? FileName { get; set; }

        [StringLength(255)]
        public string? OriginalFileName { get; set; }

        public long? FileSize { get; set; }

        [StringLength(50)]
        public string? FileType { get; set; }

        [StringLength(500)]
        public string? FilePath { get; set; }

        public DateTime? UploadedAt { get; set; }
        public int? UploadedByUserId { get; set; }
        public Document Document { get; set; }
    }
}
