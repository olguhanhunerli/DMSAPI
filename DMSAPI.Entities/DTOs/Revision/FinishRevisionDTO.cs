using DMSAPI.Entities.DTOs.DocumentDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.Revision
{
    public class FinishRevisionDTO
    {
		public IFormFile MainFile { get; set; }
		public CreateDocumentApprovalDTO Approval { get; set; }
	}
}
