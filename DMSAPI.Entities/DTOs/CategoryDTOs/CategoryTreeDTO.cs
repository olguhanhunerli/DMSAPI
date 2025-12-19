using DMSAPI.Entities.DTOs.DocumentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CategoryDTOs
{
    public class CategoryTreeDTO
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public int CompanyId { get; set; }
		public int? ParentId { get; set; }
		public int SortOrder { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		public List<CategoryTreeDTO> Children { get; set; } = new();
		public List<DocumentDTO> Documents { get; set; } = new(); 
	}
}
