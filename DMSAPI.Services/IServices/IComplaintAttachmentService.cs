using DMSAPI.Entities.DTOs.ComplaintAttachment;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IComplaintAttachmentService
    {
        Task<ComplaintAttachmentDTO> UploadAsync(string complaintNo, IFormFile file, int userId);
		Task<List<ComplaintAttachmentDTO>> GetByComplaintNoAsync(string complaintNo);
        Task<(Stream Stream, string ContentType, string DownloadFileName)> DownloadAsync(long id);
        Task<bool> DeleteAsync(long id, int userId);
	}
}
