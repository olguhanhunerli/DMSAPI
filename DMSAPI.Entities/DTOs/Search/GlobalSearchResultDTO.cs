using DMSAPI.Entities.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.Search
{
    public class GlobalSearchResultDTO
    {
		public List<CategorySearchResultDTO> Categories { get; set; } = new();
		public PagedResultDTO<DocumentSearchResultDTO> Documents { get; set; } = new();
	}
}
