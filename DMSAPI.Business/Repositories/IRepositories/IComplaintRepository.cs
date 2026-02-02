using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.ComplaintDTO;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface IComplaintRepository: IGenericRepository<Complaint>
    {
        Task<PagedResultDTO<Complaint>> GetAllComplaintsAsync(int pageNumber, int pageSize);
        Task<Complaint?> GetByComplaintNoAsync(string complaintNo);
        Task<List<ComplaintForCapaSelectDTO?>> GetForCapaSelectAsync(int companyId, string? search, int take);
    }
}
