using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int? DeletedBy { get; set; }
        public User? DeletedByUser { get; set; }
        public DateTime? DeleteAt { get; set; }
        public bool? IsDelete { get; set; }
    }
}
