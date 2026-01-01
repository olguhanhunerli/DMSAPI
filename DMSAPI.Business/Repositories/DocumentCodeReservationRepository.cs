using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories
{
	public class DocumentCodeReservationRepository : GenericRepository<DocumentCodeReservation>, IDocumentCodeReservationRepository
	{
		private readonly ICategoryRepository _categoryRepository;
		public DocumentCodeReservationRepository(DMSDbContext context, IHttpContextAccessor accessor, ICategoryRepository categoryRepository) : base(context, accessor)
		{
			_categoryRepository = categoryRepository;
		}

		public async Task<DocumentCodeReservation?> GetByCodeAsync(string documentCode)
		{
			return await _dbSet
				.Where(x => x.DocumentCode == documentCode)
				.FirstOrDefaultAsync();
		}

		public async Task MarkAsUsedAsync(string documentCode)
		{
			var reservation = await _context.DocumentCodeReservations
				.Where(x => x.DocumentCode == documentCode && !x.IsUsed)
				.FirstOrDefaultAsync();
			if (reservation != null)
			{
				reservation.IsUsed = true;
				reservation.UsedAt = DateTime.UtcNow;
				_context.DocumentCodeReservations.Update(reservation);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<DocumentCodeReservation?> ReserveNextCodeAsync(int companyId, int categoryId, string companyCode, string categoryCode, int userId)
		{
			using var trx = await _context.Database.BeginTransactionAsync();
			var rootCategory = await _categoryRepository.GetRootCategoryAsync(categoryId);

			var rootId = rootCategory.Id;
			var rootCode = rootCategory.Code;

			var lastReservation = await _dbSet
				.Where(x => x.CompanyId == companyId && x.RootCategoryId == rootId)
				.OrderByDescending(x => x.SequenceNumber)
				.Select(x => (int?)x.SequenceNumber) 
				.FirstOrDefaultAsync();

			var nextSequenceNumber = (lastReservation ?? 0) + 1; 

			var code = $"{companyCode}-{rootCode}-{nextSequenceNumber:D04}";
			var reservation = new DocumentCodeReservation
			{
				CompanyId = companyId,
				CategoryId = categoryId,
				RootCategoryId = rootId,
				SequenceNumber = nextSequenceNumber,
				DocumentCode = code,
				ReservedByUserId = userId,
				ReservedAt = DateTime.UtcNow,
				IsUsed = false
			};
			await _dbSet.AddAsync(reservation);
			await _context.SaveChangesAsync();
			await trx.CommitAsync();
			return reservation;
		}
	}
}
