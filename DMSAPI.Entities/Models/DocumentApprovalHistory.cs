using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class DocumentApprovalHistory
    {
        [Key]
		public int Id { get; set; }

        [Required]
        public int DocumentId { get; set; }

        [StringLength(50)]
        public string? ActionType { get; set; }  

        public int? ActionByUserId { get; set; }

        public DateTime? ActionAt { get; set; }

        [StringLength(500)]
        public string? ActionNote { get; set; }

        public Document Document { get; set; }
    }
}
