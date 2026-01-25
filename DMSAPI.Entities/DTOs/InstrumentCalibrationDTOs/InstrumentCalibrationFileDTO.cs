using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.InstrumentCalibrationDTOs
{
    public class InstrumentCalibrationFileDTO
    {
		[Key]
		public int FileId { get; set; }
		public ulong CalibrationId { get; set; }

		public string FileOriginalName { get; set; } = default!;
		public string FilePath { get; set; } = default!;
		public string? FileMime { get; set; }
		public long? FileSize { get; set; }
		public string? FileType { get; set; }
		public string? Description { get; set; }
		public string CreatedByName { get; set; }
		public string UploadedByName { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; } 
	}
}
