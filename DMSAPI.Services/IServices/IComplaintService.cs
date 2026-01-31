using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.ComplaintDTO;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IComplaintService
    {
        Task<PagedResultDTO<ComplaintDTO>> GetAllComplaintsAsync(int pageNumber, int pageSize);
        Task<ComplaintDTO?> GetComplaintByNoAsync(string complaintNo);
        Task<ComplaintDTO> CreateComplaintAsync(CreateComplaintDTO createComplaintDTO, int userId, int companyId);
        Task UpdateClosedAsync(string complaintNo, int userId);
        Task DeleteComplaintAsync(string complaintNo, int userId);
        Task <ComplaintDTO> UpdateComplaintByNoAsync(string complaintNo, UpdateComplaintDTO updateComplaintDTO, int userId, int companyId);
    }
}
