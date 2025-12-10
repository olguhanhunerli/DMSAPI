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
        return query.Where(d =>
            d.IsPublic
            || d.CreatedByUserId == userId
            || (d.AllowedUsers != null &&
                d.AllowedUsers.Contains($"\"{userId}\""))
            || (d.AllowedRoles != null &&
                d.AllowedRoles.Contains($"\"{roleId}\""))
            || (d.AllowedDepartments != null &&
                d.AllowedDepartments.Contains($"\"{departmentId}\""))
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
            .Where(x => !x.IsDeleted && x.CompanyId == CompanyId);

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
            .Where(x => documentIds.Contains(x.Id) && !x.IsDeleted)
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
            .Where(x => documentIds.Contains(x.Id) && !x.IsDeleted);

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
}
