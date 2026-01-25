using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.InstrumentCalibrationDTOs
{
    public class UploadCalibrationFileDTO
    {
		public ulong CalibrationId { get; set; }

		[Required]
		public IFormFile File { get; set; } = null!;
		public string InstrumentName { get; set; }
		public string? FileType { get; set; }
		public string? Description { get; set; }
	}
}
