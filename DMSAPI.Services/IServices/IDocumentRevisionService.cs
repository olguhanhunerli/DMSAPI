using DMSAPI.Entities.DTOs.Common;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.DTOs.Revision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface IDocumentRevisionService
    {
		Task FinishReservationAsync(int documentId, int userId, string filePath, CreateDocumentApprovalDTO dto);
		Task CancelRevisiyonAsync(int documentId, int userId, string reason);
		Task StartRevisionAsync(int documentId, int userId, string revisionNote);
		Task <PagedResultDTO<MyActiveRevisionDTO>> GetMyActiveRevisionAsync(int userId, int page, int pageSize);
	}
}
