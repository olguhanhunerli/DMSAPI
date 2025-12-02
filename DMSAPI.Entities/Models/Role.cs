using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        public int? CreatedBy { get; set; }
        public int? UploadedBy { get; set; }
        public string? Description { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
