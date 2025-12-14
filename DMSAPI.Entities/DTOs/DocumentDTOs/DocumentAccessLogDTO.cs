using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentDTOs
{
	public class DocumentAccessLogDTO
	{
		public int Id { get; set; }
		public string? UserName { get; set; }
		public string? AccessType { get; set; }
		public string? IpAddress { get; set; }
		public string? AccessTypeDisplayName { get; set; }
		public string? AccessTimeText { get; set; }
	}
}
