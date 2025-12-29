using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface IGlobalSearchRepository
    {
        Task<PagedResultDTO<DocumentSearchResultDTO>> SearchDocumentsAsync(string query, int userId, int roleId, int departmentId, int page, int pageSize);
        Task<List<CategorySearchResultDTO>> SearchCategoriesAsync(string query);
	}
}
