using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CapaEvidenceFiles
{
    public class CreateCapaEvidenceFilesDTO
    {
		public IFormFile File { get; set; } = null!;
		
	}
}
