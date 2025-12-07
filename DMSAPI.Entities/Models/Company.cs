using DMSAPI.Entities.Owned;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class Company
	{
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        public string? CompanyCode { get; set; }
        public int? CreatedBy { get; set; }
        public int? UploadedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Department> Departments { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
