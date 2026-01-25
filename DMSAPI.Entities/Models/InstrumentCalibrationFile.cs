using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class InstrumentCalibrationFile
    {
		[Key]
		public ulong FileId { get; set; }

		[Required]
		public ulong CalibrationId { get; set; }

		[Required]
		[MaxLength(255)]
		public string FileOriginalName { get; set; } = default!;

		[Required]
		[MaxLength(500)]
		public string FilePath { get; set; } = default!;
		public string PdfFilePath { get; set; }
		[MaxLength(100)]
		public string? FileMime { get; set; }

		public long? FileSize { get; set; }

		[MaxLength(50)]
		public string? FileType { get; set; }

		[MaxLength(255)]
		public string? Description { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

		[Required]
		public int CompanyId { get; set; }

		public int? CreatedBy { get; set; }
		public User CreatedByName { get; set; }
		public int? UpdatedBy { get; set; }
		public User? UpdatedByName { get; set; }

		public bool IsActive { get; set; } = true;
		public bool IsDeleted { get; set; } = false;

		public DateTime? DeletedAt { get; set; }
		public int? DeletedBy { get; set; }

		public InstrumentCalibration? Calibration { get; set; }
	}
}
