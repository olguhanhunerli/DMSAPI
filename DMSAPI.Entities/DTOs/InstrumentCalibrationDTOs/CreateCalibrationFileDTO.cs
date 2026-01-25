using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.InstrumentCalibrationDTOs
{
    public class CreateCalibrationFileDTO
    {
		public ulong CalibrationId { get; set; }
		public string FileOriginalName { get; set; } = default!;
		public string FilePath { get; set; } = default!;
		public string? FileMime { get; set; }
		public long? FileSize { get; set; }
		public string? FileType { get; set; }
		public string? Description { get; set; }
	}
}
