using DMSAPI.Entities.DTOs.DocumentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IDocumentApprovalService
    {
        Task CreateApprovalFlowAsync(CreateDocumentApprovalDTO dto,int createdByUserId);
        Task ApproveAsync(int documentId, int userId);
        Task RejectAsync(int documentId, int userId, string reason);
    }
}
