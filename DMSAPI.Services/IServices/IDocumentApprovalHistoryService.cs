using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IDocumentApprovalHistoryService
    {
        Task AddAsync(DocumentApprovalHistory history);
        Task<List<DocumentApprovalHistoryDTO>> GetByDocumentIdAsync(int documentId);
    }
}
