using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.DocumentDTOs
{
    public class DownloadFileResultDTO
    {
		public byte[] FileBytes { get; set; }
		public string OriginalFileName { get; set; }
		public string ContentType { get; set; }
	}
}
