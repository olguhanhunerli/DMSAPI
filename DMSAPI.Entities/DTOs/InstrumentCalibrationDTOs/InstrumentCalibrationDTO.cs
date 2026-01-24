using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.InstrumentCalibrationDTOs
{
    public class InstrumentCalibrationDTO
    {
		public int CalibrationId { get; set; }

		public int InstrumentId { get; set; }
		public string? AssetCode { get; set; }
		public string? InstrumentName { get; set; }
		public string? SerialNo { get; set; }
		public string? Location { get; set; }

		public DateTime CalibrationDate { get; set; }
		public DateTime? DueDate { get; set; }
		public int? IntervalMonths { get; set; }

		public string Result { get; set; } = default!;
		public string? CalibrationCompany { get; set; }
		public string? CertificateNo { get; set; }

		public int CompanyId { get; set; }
		public string? CompanyName { get; set; }

		public int CreatedBy { get; set; }
		public string? CreatedByName { get; set; }

		public int? UpdatedBy { get; set; }
		public string? UpdatedByName { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
	}
}
