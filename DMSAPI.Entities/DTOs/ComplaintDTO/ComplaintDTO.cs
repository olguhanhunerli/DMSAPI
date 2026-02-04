using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.ComplaintDTO
{
    public class ComplaintDTO
    {
        public long Id { get; set; }
        public string ComplaintNo { get; set; } = null!;

        public string CompanyName { get; set; }

        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }

        public byte ChannelId { get; set; }
        public short TypeId { get; set; }
        public byte SeverityId { get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
		public string? PartNumber { get; set; }
		public string? PartRevision { get; set; }
		public string? LotNumber { get; set; }
		public string? SerialNumber { get; set; }
		public DateTime? ProductionDate { get; set; }
		public string? ProductionLine { get; set; }

		public string? CustomerComplaintNo { get; set; }
		public string? CustomerPO { get; set; }
		public string? DeliveryNoteNo { get; set; }

		public int? QuantityAffected { get; set; }
		public string? ContainmentAction { get; set; }
		public bool IsRepeat { get; set; }
        public bool NeedsCapa { get; set; }
        public bool InterimActionRequired { get; set; }
        public string? InterimActionNote { get; set; }
        public bool? IsClosed { get; set; }
        public bool? IsDeleted { get; set; }
        public string Status { get; set; }

        public DateTime ReportedAt { get; set; }

        public int CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public string? ClosedByName { get; set; }
        public int? DeletedBy  { get; set; }
        public string? DeletedByName  { get; set; }
        public int? UpdateBy { get; set; }
        public string? UpdateByName { get; set; }

        public DateTime? ClosedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<DMSAPI.Entities.DTOs.AssigneeDTO.AssigneeDTO> Assignees { get; set; } = new();
    }
}
