using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.Revision
{
    public class MyActiveRevisionDTO
    {
		public int RevisionId { get; set; }
		public int DocumentId { get; set; }
		public string DocumentCode { get; set; }
		public string DocumentTitle { get; set; }
		public int CurrentVersion { get; set; }
		public int NewVersionNumber { get; set; }
		public string CategoryName { get; set; }
		public DateTime StartedAt { get; set; }
	}
}
