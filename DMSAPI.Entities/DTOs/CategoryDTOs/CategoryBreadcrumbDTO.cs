using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CategoryDTOs
{
    public class CategoryBreadcrumbDTO
    {
		public List<BreadCrumbItemDTO> BreadcrumbList { get; set; }
		public string FullPath { get; set; }
	}
}
