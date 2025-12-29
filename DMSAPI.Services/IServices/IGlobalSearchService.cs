using DMSAPI.Entities.DTOs.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IGlobalSearchService
    {
        Task<GlobalSearchResultDTO> GlobalSearchAsync(string query, int roleId, int userId, int departmentId, int page, int pageSize);
	}
}
