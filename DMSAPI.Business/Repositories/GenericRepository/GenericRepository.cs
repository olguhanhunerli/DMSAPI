using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.Owned;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Security.Claims;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
	protected readonly DMSDbContext _context;
	protected readonly DbSet<T> _dbSet;
	protected readonly IHttpContextAccessor _http;

	protected int? CompanyId =>
		int.TryParse(_http.HttpContext?.User?.FindFirst("companyId")?.Value, out int cid)
			? cid : null;

	protected bool IsGlobalAdmin =>
		_http.HttpContext?.User?.FindFirst("role")?.Value == "GLOBAL_ADMIN";

	public GenericRepository(DMSDbContext context, IHttpContextAccessor http)
	{
		_context = context;
		_dbSet = context.Set<T>();
		_http = http;
	}

	public virtual async Task<IEnumerable<T>> GetAllAsync()
	{
		if (typeof(ICompanyOwned).IsAssignableFrom(typeof(T))
			&& !IsGlobalAdmin && CompanyId.HasValue)
		{
			return await _dbSet
				.Cast<ICompanyOwned>()
				.Where(x => x.CompanyId == CompanyId.Value)
				.Cast<T>()
				.ToListAsync();
		}

		return await _dbSet.ToListAsync();
	}

	public virtual async Task<T?> GetByIdAsync(int id)
	{
		var entity = await _dbSet.FindAsync(id);

		if (entity is ICompanyOwned owned
			&& !IsGlobalAdmin && CompanyId.HasValue
			&& owned.CompanyId != CompanyId.Value)
			return null;

		return entity;
	}

	public virtual async Task AddAsync(T entity)
	{
		if (entity is ICompanyOwned owned && !IsGlobalAdmin && CompanyId.HasValue)
			owned.CompanyId = CompanyId.Value;

		await _dbSet.AddAsync(entity);
		await _context.SaveChangesAsync();
	}

	public virtual async Task UpdateAsync(T entity)
	{
		if (entity is ICompanyOwned owned && !IsGlobalAdmin && CompanyId.HasValue)
			owned.CompanyId = CompanyId.Value;

		_dbSet.Update(entity);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(T entity)
	{
		_dbSet.Remove(entity);
		await _context.SaveChangesAsync();
	}

	public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
	{
		var query = _dbSet.Where(predicate);

		if (typeof(ICompanyOwned).IsAssignableFrom(typeof(T))
			&& !IsGlobalAdmin && CompanyId.HasValue)
		{
			query = query.Cast<ICompanyOwned>()
						 .Where(x => x.CompanyId == CompanyId.Value)
						 .Cast<T>();
		}

		return await query.ToListAsync();
	}

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}
