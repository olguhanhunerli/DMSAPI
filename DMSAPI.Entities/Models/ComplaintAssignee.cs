using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class ComplaintAssignee
    {
        public long Id { get; set; }
        public long ComplaintId { get; set; }
        public int UserId { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime AssignedAt { get; set; }
        public int? AssignedBy { get; set; }

        public Complaint Complaint { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
