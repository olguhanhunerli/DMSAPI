using DMSAPI.Entities.Owned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class Category: ICompanyOwned
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string? Description { get; set; }
		public string? Slug { get; set; }
		public string? Code { get; set; }

		public int? ParentId { get; set; }
		public Category? Parent { get; set; }
		public ICollection<Category>? Children { get; set; }

		public int CompanyId { get; set; }
		public Company Company { get; set; }

		public int SortOrder { get; set; } = 0;
		public bool IsActive { get; set; } = true;
		public bool IsDeleted { get; set; } = false;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; }
		public int? CreatedBy { get; set; }
		public User CreatedByUser { get; set; }
		public int? UpdatedBy { get; set; }
        public User UpdatedByUser { get; set; }

    }
}
