using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class DocumentCodeReservation
    {
		public int Id { get; set; }

		public int CompanyId { get; set; }
		public int CategoryId { get; set; }

		public string DocumentCode { get; set; } = null!;
		public int SequenceNumber { get; set; }

		public bool IsUsed { get; set; } = false;

		public int ReservedByUserId { get; set; }
		public int RootCategoryId { get; set; }
		public DateTime ReservedAt { get; set; }

		public DateTime? UsedAt { get; set; }
	}
}
