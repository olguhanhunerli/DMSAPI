using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.PositionDTOs
{
    public class PositionDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
		public DateTime UploadedAt { get; set; }
		public int? UploadedBy { get; set; }
		public string? UploadedByName { get; set; }
	}
}
