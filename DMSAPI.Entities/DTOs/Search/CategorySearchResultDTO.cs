using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.Search
{
    public class CategorySearchResultDTO
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Code { get; set; }
		public string FullPath { get; set; }
	}
}
