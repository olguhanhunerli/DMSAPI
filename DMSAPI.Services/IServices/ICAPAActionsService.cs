using DMSAPI.Entities.DTOs.CapaActionFileDTO;
using DMSAPI.Entities.DTOs.CAPADTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface ICAPAActionsService
    {
        Task<CAPAActionDTO> CreateActionAsync(string capaNo, CreateCAPAActionDTO action, int userId, int companyId);
        Task<CAPAActionDTO> UpdateActionAsync(long actionId,UpdateCAPAActionDTO action, int userId, int companyId);
        Task<List<CAPAActionDTO>> GetByCapaNoAsync(string capaNo, int companyId);
		Task<bool> UploadActionFileAsync(long actionId, CreateActionFilesDTO dto, int userId);
        Task<(string FullPath, string FileName, string ContentType)> DownloadActionFileAsync(long actionId, long fileId);
	}
}
