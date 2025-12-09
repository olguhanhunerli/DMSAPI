using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentDTOs
{
    public class DocumentApprovalHistoryDTO
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }

        public string ActionType { get; set; }

        public int? ActionByUserId { get; set; }
        public string ActionByName { get; set; }

        public DateTime ActionAt { get; set; }
        public string ActionNote { get; set; }

        public string ActionDisplayName { get; set; }  
        public string ActionTimeText { get; set; }
    }
}
