using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CAPADTO
{
    public class CAPAActionDTO
    {
        public long Id { get; set; }
        public string CapaNo { get; set; } = null!;
        public string ActionType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }  
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = null!;
        public bool EvidenceRequired { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
