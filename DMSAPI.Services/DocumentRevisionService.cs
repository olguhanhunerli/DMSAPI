using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.DTOs.Revision;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services
{
	public class DocumentRevisionService : IDocumentRevisionService
	{
		private readonly IDocumentRevisionRepository _repository;
		private readonly IDocumentRepository _documentRepository;
		private readonly DMSDbContext _context;
		private readonly IDocumentVersionService _documentVersionService;
		private readonly IDocumentApprovalRepository _documentApprovalRepository;
		private readonly IDocumentApprovalHistoryService _documentApprovalHistoryService;
		private readonly IDocumentApprovalService _documentApprovalService;
		public DocumentRevisionService(IDocumentRevisionRepository repository, IDocumentRepository documentRepository, DMSDbContext context, IDocumentVersionService documentVersionService, IDocumentApprovalHistoryService documentApprovalHistoryService, IDocumentApprovalRepository documentApprovalRepository, IDocumentApprovalService documentApprovalService)
		{
			_repository = repository;
			_documentRepository = documentRepository;
			_context = context;
			_documentVersionService = documentVersionService;
			_documentApprovalHistoryService = documentApprovalHistoryService;
			_documentApprovalRepository = documentApprovalRepository;
			_documentApprovalService = documentApprovalService;
		}

		public async Task CancelRevisiyonAsync(int documentId, int userId, string reason)
		{
			using var trx = await _context.Database.BeginTransactionAsync();
			var revision = await _repository.GetActiveByDocumentIdAsync(documentId)
				?? throw new InvalidOperationException("No active revision found for this document.");

			if(revision.StartedByUserId != userId)
			{
				throw new UnauthorizedAccessException("Only the user who started the revision can cancel it.");
			}
			var document = await _documentRepository.GetByIdAsync(documentId);
			if (document == null)
			{
				throw new InvalidOperationException("Document not found.");
			}
			revision.IsActive = false;
			revision.Status = "Cancelled";
			revision.CompletedAt = DateTime.UtcNow;
			revision.RevisionNote = reason;

			document.IsLocked = false;
			document.LockedByUserId = null;
			document.LockedAt = null;

			await _context.SaveChangesAsync();
			await trx.CommitAsync();
		}

		public async Task FinishReservationAsync(int documentId, int userId, string filePath, CreateDocumentApprovalDTO dto)
		{
			using var trx = await _context.Database.BeginTransactionAsync();
			var revision = await _repository.GetActiveByDocumentIdAsync(documentId)
				?? throw new InvalidOperationException("No active revision found for this document.");
			if (revision.StartedByUserId != userId)
			{
				throw new UnauthorizedAccessException("Only the user who started the revision can finish it.");
			}
			var document = await _documentRepository.GetByIdAsync(documentId);
			if (document == null)
			{
				throw new InvalidOperationException("Document not found.");
			}
			revision.IsActive = false;
			revision.Status = "Completed";
			revision.CompletedAt = DateTime.UtcNow;

			await _documentVersionService.CreateVersionFromRevisionAsync(revision, filePath, userId);
			document.VersionNumber = revision.NewVersionNumber;
			document.IsLocked = false;
			document.LockedByUserId = null;
			document.LockedAt = null;

			var oldApprovals = await _documentApprovalRepository.GetByDocumentIdAsync(documentId);
			foreach (var old in oldApprovals)
			{
				await _documentApprovalHistoryService.AddAsync(new DocumentApprovalHistory
				{
					DocumentId = documentId,
					ActionType = "APPROVAL_RESET",
					ActionByUserId = userId,
					ActionAt = DateTime.UtcNow,
					ActionNote = $"Revision Sonrası Approval Resetlendi (Level {old.ApprovalLevel})"
				});
				await _documentApprovalRepository.DeleteAsync(old);
			}
			if (dto != null && dto.Approvers.Any())
			{
				dto.DocumentId = documentId;
				await _documentApprovalService.CreateApprovalFlowAsync(dto, userId);

				await _documentApprovalHistoryService.AddAsync(new DocumentApprovalHistory
				{
					DocumentId = documentId,
					ActionType = "SENT_FOR_APPROVAL",
					ActionByUserId = userId,
					ActionAt = DateTime.UtcNow,
					ActionNote = "Revision Sonrası Yeni Approval Flow Oluşturuldu"
				});
			}
			await _context.SaveChangesAsync();
			await trx.CommitAsync();
		}

		public async Task<PagedResultDTO<MyActiveRevisionDTO>> GetMyActiveRevisionAsync(int userId, int page, int pageSize)
		{
			var (items, totalCount) = await _repository.GetMyActiveRevisionAsync(userId, page, pageSize);
			var dtoItems = items.Select(r => new MyActiveRevisionDTO
			{
				RevisionId = r.Id,
				DocumentId = r.DocumentId,
				DocumentTitle = r.Document.Title,
				CategoryName = r.Document.Category.Name,
				StartedAt = r.StartedAt,
				DocumentCode = r.Document.DocumentCode,
				CurrentVersion = r.Document.VersionNumber,
				NewVersionNumber = r.Document.VersionNumber +1,
				
			}).ToList();
			return new PagedResultDTO<MyActiveRevisionDTO>
			{
				Items = dtoItems,
				TotalCount = totalCount,
				Page = page,
				PageSize = pageSize
			};
		}

		public async Task StartRevisionAsync(int documentId, int userId, string revisionNote)
		{
			var active = await _repository.GetActiveByDocumentIdAsync(documentId);
			if (active != null)
			{
				throw new InvalidOperationException("There is already an active revision for this document.");
			}
			var document = await _documentRepository.GetByIdAsync(documentId);
			if (document == null)
			{
				throw new InvalidOperationException("Document not found.");
			}
			var revision = new DocumentRevision
			{
				DocumentId = documentId,
				StartedByUserId = userId,
				StartedAt = DateTime.UtcNow,
				RevisionNote = revisionNote,
				IsActive = true,
				Status = "In Progress"
			};

			await _repository.AddAsync(revision);
			document.IsLocked = true;
			document.LockedByUserId = userId;
			document.LockedAt = DateTime.UtcNow;

			await _documentRepository.UpdateAsync(document);
		}
	}
}
