using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ManagerId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
