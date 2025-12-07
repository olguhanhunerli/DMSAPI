using DMSAPI.Entities.Owned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class Permission : ICompanyOwned
	{
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int? FolderId { get; set; }
        public int CompanyId { get; set; }
        public int? DocumentId { get; set; }
        public bool CanView { get; set; }
        public bool CanUpload { get; set; }
        public bool CanDownload { get; set; }
        public bool CanShare { get; set; }
        public bool CanDelete { get; set; }
        public bool CanApprove { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
