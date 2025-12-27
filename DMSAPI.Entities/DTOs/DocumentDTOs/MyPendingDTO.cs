using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentDTOs
{
    public class MyPendingDTO
    {
        public int Id { get; set; }

        public string DocumentCode { get; set; } = null!;
        public string Title { get; set; } = null!;

        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }

        public int WaitingDays   => (DateTime.UtcNow - CreatedAt).Days;
    }
}
