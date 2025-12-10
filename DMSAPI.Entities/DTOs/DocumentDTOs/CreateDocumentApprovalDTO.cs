using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentDTOs
{
    public class CreateDocumentApprovalDTO
    {
        public int DocumentId { get; set; }

        public List<ApprovalUserLevelDTO> Approvers { get; set; } = new();
    }
}
