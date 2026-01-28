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
        Task<ComplaintDTO?> GetComplaintByIdAsync(long id);
        Task<ComplaintDTO> CreateComplaintAsync(CreateComplaintDTO createComplaintDTO, int userId);
        Task UpdateClosedAsync(int id, int userId);
        Task DeleteComplaintAsync(int id, int userId);
        Task <ComplaintDTO> UpdateComplaintAsync(int id, UpdateComplaintDTO updateComplaintDTO, int userId);
    }
}
