using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CAPADTO
{
    public class UpdateCAPAActionDTO
    {
        public string? Description { get; set; }
        public int? OwnerId { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }

        public string? CompletionNote { get; set; }
        public bool? EvidenceRequired { get; set; }
    }
}
