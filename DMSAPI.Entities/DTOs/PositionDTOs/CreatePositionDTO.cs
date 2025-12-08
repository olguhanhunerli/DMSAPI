using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.PositionDTOs
{
    public class CreatePositionDTO
    {
		public string Name { get; set; }
		public string Description { get; set; }
		public int CompanyId { get; set; }
		public bool IsActive { get; set; }
		
	}
}
