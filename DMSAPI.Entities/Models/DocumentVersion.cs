using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class DocumentVersion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DocumentId { get; set; }

        public int? VersionNumber { get; set; }

        [StringLength(255)]
        public string? FileName { get; set; }

        [StringLength(255)]
        public string? OriginalFileName { get; set; }

        public long? FileSize { get; set; }

        [StringLength(50)]
        public string? FileType { get; set; }

        [StringLength(500)]
        public string? VersionNote { get; set; }

        public DateTime? CreatedAt { get; set; }
        public int? CreatedByUserId { get; set; }

        // Navigation
        public Document Document { get; set; }
    }
}
