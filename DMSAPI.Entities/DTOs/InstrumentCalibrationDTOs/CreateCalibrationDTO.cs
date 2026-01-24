using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.InstrumentCalibrationDTOs
{
    public class CreateCalibrationDTO
    {
		[Required]
		public ulong InstrumentId { get; set; }
		[Required]
		public DateTime CalibrationDate { get; set; }   

		[Range(1, 120)]
		public int IntervalMonths { get; set; } = 12;

		[Required]
		[MaxLength(20)]
		public string Result { get; set; } = default!;  

		[MaxLength(255)]
		public string? CalibrationCompany { get; set; }

		[MaxLength(100)]
		public string? CertificateNo { get; set; }

		[Required]
		public int CompanyId { get; set; }

		[MaxLength(150)]
		public string? Location { get; set; }
	}
}
