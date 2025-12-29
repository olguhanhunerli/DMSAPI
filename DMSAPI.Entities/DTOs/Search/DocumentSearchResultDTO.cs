using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.Search
{
    public class DocumentSearchResultDTO
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public string? DocumentCode { get; set; }
		public string CategoryPath { get; set; }
	}
}
