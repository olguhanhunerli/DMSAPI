using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class DocumentRevision
	{
		public int Id { get; set; }

		public int DocumentId { get; set; }
		public Document Document { get; set; }

		public int OldVersionNumber { get; set; }
		public int NewVersionNumber { get; set; }
		public bool IsActive { get; set; }
		public string? RevisionNote { get; set; }

		public int StartedByUserId { get; set; }
		public User StartedByUser { get; set; }

		public DateTime StartedAt { get; set; }
		public DateTime? CompletedAt { get; set; }

		public string Status { get; set; }

	}
}
