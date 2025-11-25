using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
