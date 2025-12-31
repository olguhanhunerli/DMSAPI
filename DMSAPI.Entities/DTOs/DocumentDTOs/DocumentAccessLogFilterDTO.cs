using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentDTOs
{
    public class DocumentAccessLogFilterDTO
    {
        public int? DocumentId { get; set; }
        public int? UserId { get; set; }

        public string? AccessType { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
