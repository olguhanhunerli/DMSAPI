using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class InstrumentCalibration
	{
		[Key]
		public ulong CalibrationId { get; set; }   

		[Required]
		public int InstrumentId { get; set; }

		[ForeignKey(nameof(InstrumentId))]
		public Instrument InstrumentName { get; set; } = null!;

		[Required]
		public DateTime CalibrationDate { get; set; }

		public int? IntervalMonths { get; set; }
		public DateTime? DueDate { get; set; }

		[MaxLength(255)]
		public string? CalibrationCompany { get; set; }

		[MaxLength(100)]
		public string? CertificateNo { get; set; }

		[Required, MaxLength(20)]
		public string Result { get; set; } 

		public string? Notes { get; set; }

		[Required]
		public int CompanyId { get; set; }

		[ForeignKey(nameof(CompanyId))]
		public Company CompanyName { get; set; } = null!;

		public int CreatedBy { get; set; }

		[ForeignKey(nameof(CreatedBy))]
		public User CreatedByName { get; set; } = null!;

		public int? UpdatedBy { get; set; }

		[ForeignKey(nameof(UpdatedBy))]
		public User? UpdatedByName { get; set; }

		public bool IsActive { get; set; } = true;
		public bool IsDeleted { get; set; } = false;

		public int? DeletedBy { get; set; }

		[ForeignKey(nameof(DeletedBy))]
		public User? DeletedByUser { get; set; }

		public DateTime? DeletedAt { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		public ICollection<InstrumentCalibrationFile> Files { get; set; } = new List<InstrumentCalibrationFile>();
	}
}
