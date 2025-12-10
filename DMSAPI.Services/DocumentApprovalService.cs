using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;

public class DocumentApprovalService : IDocumentApprovalService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentApprovalRepository _documentApprovalRepository;

    public DocumentApprovalService(
        IDocumentRepository documentRepository,
        IDocumentApprovalRepository documentApprovalRepository)
    {
        _documentRepository = documentRepository;
        _documentApprovalRepository = documentApprovalRepository;
    }

    public async Task ApproveAsync(int documentId, int userId)
    {
        var currentApproval =
            await _documentApprovalRepository.GetNextPendingApprovalAsync(documentId);

        if (currentApproval == null)
            throw new Exception("Bekleyen onay bulunamadı.");

        if (currentApproval.UserId != userId)
            throw new Exception("Bu onay size ait değil.");

        currentApproval.IsApproved = true;
        currentApproval.ActionAt = DateTime.UtcNow;

        await _documentApprovalRepository.UpdateAsync(currentApproval);

        var nextApproval =
            await _documentApprovalRepository.GetNextPendingApprovalAsync(documentId);

        var document = await _documentRepository.GetByIdAsync(documentId);
        if (document == null)
            throw new Exception("Doküman bulunamadı.");

        if (nextApproval == null)
        {
            document.StatusId = 2; 
        }
        else
        {
            document.StatusId = 1; 
        }

        await _documentRepository.UpdateAsync(document);
    }

    public async Task CreateApprovalFlowAsync(CreateDocumentApprovalDTO dto, int createdByUserId)
    {
        if (!dto.Approvers.Any())
            throw new Exception("En az 1 onaylayıcı zorunludur.");

        if (dto.Approvers.GroupBy(x => x.ApprovalLevel).Any(g => g.Count() > 1))
            throw new Exception("Aynı seviyede birden fazla kişi olamaz.");

        var document = await _documentRepository.GetByIdAsync(dto.DocumentId);
        if (document == null)
            throw new Exception("Doküman bulunamadı.");

        var oldApprovals =
            await _documentApprovalRepository.GetByDocumentIdAsync(dto.DocumentId);

        foreach (var old in oldApprovals)
            await _documentApprovalRepository.DeleteAsync(old);

        foreach (var approver in dto.Approvers)
        {
            var entity = new DocumentApproval
            {
                DocumentId = dto.DocumentId,
                UserId = approver.UserId,
                ApprovalLevel = approver.ApprovalLevel,
                CreatedAt = DateTime.UtcNow,
                IsApproved = false,
                IsRejected = false
            };

            await _documentApprovalRepository.AddAsync(entity);
        }

        document.StatusId = 1; 
        await _documentRepository.UpdateAsync(document);
    }

    public async Task RejectAsync(int documentId, int userId, string reason)
    {
        var approval =
            await _documentApprovalRepository.GetNextPendingApprovalAsync(documentId);

        if (approval == null)
            throw new Exception("Onay bekleyen dosya yok.");

        if (approval.UserId != userId)
            throw new Exception("Bu onay size ait değil.");

        approval.IsRejected = true;
        approval.RejectReason = reason;
        approval.ActionAt = DateTime.UtcNow;

        await _documentApprovalRepository.UpdateAsync(approval);

        var document = await _documentRepository.GetByIdAsync(documentId);
        if (document == null)
            throw new Exception("Doküman bulunamadı.");

        document.StatusId = 3;

        await _documentRepository.UpdateAsync(document);
    }
}
