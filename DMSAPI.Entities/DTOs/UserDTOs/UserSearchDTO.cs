using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.UserDTOs
{
    public class UserSearchDTO
    {
		public string? Keyword { get; set; }
		public int? RoleId { get; set; }
		public int? DepartmentId { get; set; }
		public int? CompanyId { get; set; }
		public bool? IsActive { get; set; }
	}
}
