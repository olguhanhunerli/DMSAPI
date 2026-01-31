using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.ComplaintDTO
{
    public class CreateComplaintDTO
    {
		public int CompanyId { get; set; } 

		[Required]
		public int CustomerId { get; set; }

		[Required]
		public byte ChannelId { get; set; }

		[Required]
		public short TypeId { get; set; }

		[Required]
		public byte SeverityId { get; set; }

		[Required, MaxLength(200)]
		public string Title { get; set; } = null!;

		[Required]
		public string Description { get; set; } = null!;

		public bool IsRepeat { get; set; } = false;

		public bool InterimActionRequired { get; set; } = false;
		public string? InterimActionNote { get; set; }

		public int? AssignedTo { get; set; }

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
	}
}
