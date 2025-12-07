using DMSAPI.Entities.Owned;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class Role : ICompanyOwned
	{
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        public int CompanyId { get; set; }
        public int? CreatedBy { get; set; }
        public User? CreatedByUser { get; set; }
        public int? UploadedBy { get; set; }
        public User? UploadedByUser { get; set; }
        public string? Description { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
