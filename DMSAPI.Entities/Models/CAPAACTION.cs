using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class CAPAACTION
    {
        public long Id { get; set; }

        public string CapaNo { get; set; } = null!;

        public string ActionType { get; set; } = null!;

        public string Description { get; set; } = null!; 

        public int OwnerId { get; set; }

        public DateTime? DueDate { get; set; }

        public string Status { get; set; } 

        public DateTime? CompletedAt { get; set; }
        public int? CompletedBy { get; set; }

        public string? CompletionNote { get; set; } 

        public bool EvidenceRequired { get; set; } = false;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(CapaNo))]
        public CAPA? CAPA { get; set; }

        public User? OwnerByUser { get; set; }
        public User? CompletedByUser { get; set; }
		public ICollection<ActionFile> Files { get; set; } = new List<ActionFile>();
	}
}
