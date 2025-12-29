using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Search;
using DMSAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
	public class GlobalSearchService : IGlobalSearchService
	{
		private readonly IGlobalSearchRepository _repository;

		public GlobalSearchService(IGlobalSearchRepository repository)
		{
			_repository = repository;
		}

		public async Task<GlobalSearchResultDTO> GlobalSearchAsync(string query, int roleId, int userId, int departmentId, int page, int pageSize)
		{
			if(string.IsNullOrWhiteSpace(query))
			{
				return new GlobalSearchResultDTO();
			}
			var categoriesTask = await _repository.SearchCategoriesAsync(query);
			var documentTask = await _repository.SearchDocumentsAsync(query, roleId, userId, departmentId, page, pageSize);

			return new GlobalSearchResultDTO
			{
				Categories = categoriesTask,
				Documents = documentTask
			};
		}
	}
}
