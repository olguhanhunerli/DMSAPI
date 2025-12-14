using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class DocumentApproval
    {
        public int Id { get; set; }

        public int DocumentId { get; set; }

        public int UserId { get; set; }

        public int ApprovalLevel { get; set; }

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        public DateTime? ActionAt { get; set; }

        public string? RejectReason { get; set; }

        public DateTime CreatedAt { get; set; }


        public Document Document { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}
