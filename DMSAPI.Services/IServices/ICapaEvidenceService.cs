using DMSAPI.Entities.DTOs.CapaEvidenceFiles;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.IServices
{
    public interface ICapaEvidenceService
    {
        Task<CapaEvidenceFiles> CreateFiles(string capaNo, CreateCapaEvidenceFilesDTO dto, int userId);
		Task<(string FullPath, string FileName, string ContentType)> DownloadFileAsync(string capaNo, long fileId);
	}
}
