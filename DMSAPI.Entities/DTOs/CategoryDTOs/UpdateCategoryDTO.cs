using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CategoryDTOs
{
    public class UpdateCategoryDTO
    {
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public int? ParentId { get; set; }
		public int? SortOrder { get; set; }
		public bool? IsActive { get; set; }
		public int UpdatedBy { get; set; }
	}
}
