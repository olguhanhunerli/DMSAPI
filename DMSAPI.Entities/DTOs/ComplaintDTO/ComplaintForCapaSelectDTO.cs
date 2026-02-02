using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.ComplaintDTO
{
    public class ComplaintForCapaSelectDTO
    {
        public long Id { get; set; }
        public string ComplaintNo { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? CustomerName { get; set; }
        public byte SeverityId { get; set; }
        public DateTime ReportedAt { get; set; }
    }
}
