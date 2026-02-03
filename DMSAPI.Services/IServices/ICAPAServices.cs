using DMSAPI.Entities.DTOs.CAPADTO;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface ICAPAServices
    {
        Task<PagedResultDTO<CAPADTO>> GetCapaPagedResult(int page, int pageSize);
        Task<CAPADTO> GetCAPAByCapaNoAsync(string capaNo);
        Task<List<RootCauseMethod>> GetRootCouseMethodAsync();
        Task<CAPADTO> CreateCapaAsync(CreateCAPADTO createCAPADTO, int userId, int companyId);
        Task<CapaCreateFormInitDTO> GetCreateFormInitAsync(string complaintNo, int userId, int companyId);

    }
}
