using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories
{
	public class GlobalSearchRepository : IGlobalSearchRepository
	{
		private readonly DMSDbContext _context;
		private readonly IHttpContextAccessor _http;

		public GlobalSearchRepository(DMSDbContext context, IHttpContextAccessor http)
		{
			_context = context;
			_http = http;
		}

		public async Task<List<CategorySearchResultDTO>> SearchCategoriesAsync(string query)
		{
			return await _context.Categories
				.AsNoTracking()
				.Where(c =>
				!c.IsDeleted &&
				c.CompanyId == CompanyId &&
				(c.Name.Contains(query) ||
				c.Code.Contains(query))
				).Select(c => new CategorySearchResultDTO
				{
					Id = c.Id,
					Name = c.Name,
					Code = c.Code,
				}).ToListAsync();
		}

		public async Task<PagedResultDTO<DocumentSearchResultDTO>> SearchDocumentsAsync(string query,int userId,int roleId,int departmentId,int page,int pageSize)
					{
			var docs = _context.Documents
				.AsNoTracking()
				.Include(x => x.Category)
				.Where(d =>
					!d.IsDeleted &&
					d.StatusId == 2 &&
					d.CompanyId == CompanyId
				);

			var dept = departmentId.ToString();

			docs = docs.Where(d =>
				d.IsPublic
				|| (
					d.AllowedDepartments != null &&
					d.AllowedDepartments != "[]" &&
					(
						d.AllowedDepartments == $"[{dept}]"
						|| d.AllowedDepartments.StartsWith($"[{dept},")
						|| d.AllowedDepartments.Contains($",{dept},")
						|| d.AllowedDepartments.EndsWith($",{dept}]")
					)
				)
			);
			docs = docs.Where(d =>
				d.Title.Contains(query) ||
				d.DocumentCode.Contains(query) ||
				d.Category.Name.Contains(query)
			);

			var totalCount = await docs.CountAsync();

			var items = await docs
				.OrderByDescending(d => d.CreatedAt)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(d => new DocumentSearchResultDTO
				{
					Id = d.Id,
					Title = d.Title,
					DocumentCode = d.DocumentCode,
				})
				.ToListAsync();

			return new PagedResultDTO<DocumentSearchResultDTO>
			{
				TotalCount = totalCount,
				Page = page,
				PageSize = pageSize,
				Items = items
			};
		}
		private int? CompanyId =>
			int.TryParse(_http.HttpContext?.User?.FindFirst("companyId")?.Value, out var companyId) ? companyId : null;
	}
}
