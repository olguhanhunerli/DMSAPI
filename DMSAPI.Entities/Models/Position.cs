using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class Position
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

       

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
