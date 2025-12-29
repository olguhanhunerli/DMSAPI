using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
{
    public DocumentRepository(DMSDbContext context, IHttpContextAccessor accessor)
        : base(context, accessor)
    {
    }

    private IQueryable<Document> ApplyAccessFilter(
      IQueryable<Document> query,
      int userId,
      int roleId,
      int departmentId)
    {
        var dept = departmentId.ToString();
        var role = roleId.ToString();
        var user = userId.ToString();

        return query.Where(d =>
            d.CompanyId == CompanyId
            &&
            (
                d.IsPublic

                || (
                    d.AllowedDepartments != null
                    && d.AllowedDepartments != "[]"

                    && (
                        d.AllowedDepartments == $"[{dept}]"
                        || d.AllowedDepartments.StartsWith($"[{dept},")
                        || d.AllowedDepartments.Contains($",{dept},")
                        || d.AllowedDepartments.EndsWith($",{dept}]")
                    )
                )
            )
        );
    }

    public async Task<bool> DocumentCodeExistingAsync(string documentCode)
    {
        return await _dbSet.AnyAsync(d =>
            d.DocumentCode == documentCode &&
            d.CompanyId == CompanyId &&
            !d.IsDeleted
        );
    }


    public async Task<int> GetNextDocumentNumberAsync(int companyId, int categoryId)
    {
        var lastDocCode = await _dbSet
            .Where(d => d.CompanyId == companyId
                     && d.CategoryId == categoryId
                     && !d.IsDeleted)
            .OrderByDescending(d => d.Id)
            .Select(d => d.DocumentCode)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(lastDocCode))
            return 1;

        var parts = lastDocCode.Split('-');
        var numberPart = parts.Last();

        if (int.TryParse(numberPart, out int number))
            return number + 1;

        return 1;
    }


    public async Task<PagedResultDTO<Document>> GetPagedAuthorizedAsync(
        int page,
        int pageSize,
        int userId,
        int roleId,
        int departmentId)
    {
        var query = _dbSet
             .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.Company)
            .Include(x => x.CreatedByUser)
            .Include(x => x.Approvals)
                .ThenInclude(a => a.User)
            .Where(x => !x.IsDeleted && x.CompanyId == CompanyId && x.StatusId == 2);

        query = ApplyAccessFilter(query, userId, roleId, departmentId);

        query = query
            .Include(x => x.Approvals)
                .ThenInclude(a => a.User);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDTO<Document>
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }


    public async Task<bool> ValidateDocumentCodeAsync(
        string documentCode,
        int companyId,
        int categoryId)
    {
        var parts = documentCode.Split('-', 3);
        if (parts.Length != 3)
            return false;

        var companyCode = parts[0];
        var categoryCode = parts[1];
        var numberPart = parts[2];

        var company = await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == companyId);

        if (company == null ||
            !string.Equals(company.CompanyCode, companyCode, StringComparison.OrdinalIgnoreCase))
            return false;

        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == categoryId);

        if (category == null ||
            !string.Equals(category.Code, categoryCode, StringComparison.OrdinalIgnoreCase))
            return false;

        if (!int.TryParse(numberPart, out _))
            return false;

        return true;
    }

    public async Task<List<Document>> GetPendingDocumentIdsForUserAsync(List<int> documentIds)
    {
        return await _dbSet
                .Where(x =>
                    documentIds.Contains(x.Id)
                    && !x.IsDeleted
                    && x.CompanyId == CompanyId)
                .Include(x => x.Approvals)
                    .ThenInclude(a => a.User)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
    }

    public async Task<PagedResultDTO<Document>> GetPagedPendingByIdsAsync(
    List<int> documentIds,
    int page,
    int pageSize)
    {
        var query = _dbSet
            .AsNoTracking()

            .Where(x => documentIds.Contains(x.Id) && !x.IsDeleted && x.CompanyId == CompanyId)
            .Include(x => x.Approvals)
                .ThenInclude(a => a.User)
            .Include(x => x.Attachments)
            .Include(x => x.CreatedByUser)
            .Include(x => x.Files)
            .Include(x => x.Versions)
            .Include(x => x.ApprovalHistories)
            .Include(x => x.AccessLogs);

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDTO<Document>
        {
            TotalCount = total,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task AddMainFileAsync(DocumentFile file)
    {
        await _context.DocumentFiles.AddAsync(file);
        await _context.SaveChangesAsync();
    }

    public async Task<DocumentFile?> GetMainFileAsync(int documentId)
    {
        return await _context.DocumentFiles
        .Where(x => x.DocumentId == documentId)
        .OrderByDescending(x => x.Id)
        .FirstOrDefaultAsync();
    }

    public async Task<PagedResultDTO<Document>> GetPagedApprovedAsync(int page, int pageSize, int userId, int roleId, int departmentId)
    {
        var query = _dbSet
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.Company)
            .Include(x => x.CreatedByUser)
            .Where(x => !x.IsDeleted && x.StatusId == 2 && x.CompanyId == CompanyId);
        query = ApplyAccessFilter(query, userId, roleId, departmentId);
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDTO<Document>
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<Document?> GetDetailByIdAsync(int documentId)
    {
        return await _dbSet
        .AsNoTracking()
        .Include(x => x.Category)
        .Include(x => x.Company)
        .Include(x => x.CreatedByUser)
        .Include(x => x.UpdatedByUser)
        .Include(x => x.DeletedByUser)
        .Include(x => x.Approvals)
            .ThenInclude(a => a.User)
        .Include(x => x.Files)
        .Include(x => x.Attachments)
        .Include(x => x.Versions)
        .Include(x => x.ApprovalHistories)
        .Include(x => x.AccessLogs)
        .FirstOrDefaultAsync(x => x.Id == documentId && !x.IsDeleted && x.CompanyId == CompanyId);
    }

    public async Task<PagedResultDTO<Document>> GetPagedRejectedAsync(int page, int pageSize)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var query = _dbSet
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.Company)
            .Include(x => x.CreatedByUser)
            .Include(x => x.Approvals)
                .ThenInclude(x => x.User)
            .Where(x =>
                !x.IsDeleted &&
                x.CompanyId == CompanyId &&
                (
                    x.StatusId == 3 ||
                    x.Approvals.Any(a => a.IsRejected)
                )
            );

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDTO<Document>
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<Document?> GetDocumentWithFileAsync(int documentId)
    {
        return await _dbSet
            .Include(x => x.Files)
            .Include(x => x.Category)
            .Include(x => x.Company)
            .FirstOrDefaultAsync(x => x.Id == documentId && !x.IsDeleted && x.CompanyId == CompanyId);
    }

	public async Task<PagedResultDTO<Document>> GetPagedByCategoryAsync(int page, int pageSize, int? categoryId, int roleId, int userId, int departmentId)
	{
		var query = _dbSet
            .Include(x => x.Category)
            .Include(x => x.Company)
			.Include(x => x.CreatedByUser)
			.Where(x => !x.IsDeleted && x.CompanyId == CompanyId && x.StatusId == 2);
		if (categoryId.HasValue)
		{
			query = query.Where(x => x.CategoryId == categoryId.Value);
		}
		query = ApplyAccessFilter(query, userId, roleId, departmentId);
		var totalCount = await query.CountAsync();
		var items = await query
			.OrderByDescending(x => x.CreatedAt)
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();
		return new PagedResultDTO<Document>
		{
			TotalCount = totalCount,
			Page = page,
			PageSize = pageSize,
			Items = items
		};
	}
}
